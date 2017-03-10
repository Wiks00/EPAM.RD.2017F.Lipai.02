using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Logger;
using MasterSlaveUserService.Exceptions;
using MasterSlaveUserService.Interfaces;
using ServiceConfiguration;
using ServiceLibrary;
using ServiceLibrary.Interfaces;

namespace MasterSlaveUserService
{
    public class MasterService : MarshalByRefObject, IMasterService
    {
        private readonly Func<long?> generateIdentifierFunction;
        private readonly IEqualityComparer<User> userEqualityComparer;
        private readonly ILogger logger;

        private readonly ReaderWriterLockSlim rwLockSlim = new ReaderWriterLockSlim();
        private ICollection<User> storage;

        public MasterService() : this(null, null)
        {
        }

        public MasterService(IEqualityComparer<User> userEqualityComparer) : this(null, userEqualityComparer)
        {
        }

        public MasterService(Func<long?> generateIdentifierFunction) : this(generateIdentifierFunction, null)
        {
        }

        public MasterService(Func<long?> generateIdentifierFunction,
                            IEqualityComparer<User> userEqualityComparer,
                            ILogger logger = null)
        {
            if (ReferenceEquals(generateIdentifierFunction, null))
            {
                this.generateIdentifierFunction = () => storage?.Max(user => user.Identifier) + 1 ?? 0;
            }
            else
            {
                this.generateIdentifierFunction = generateIdentifierFunction;
            }

            this.userEqualityComparer = userEqualityComparer ?? new UserEqualityComparer();
            this.storage = new HashSet<User>(this.userEqualityComparer);

            try
            {
                this.IsLogging = Configurator.GetLoggerConfig;

                if (this.IsLogging)
                {
                    this.logger = logger ?? NLogLogger.Logger;
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new UserServiceException("Can't get settings", ex);
            }
            catch (FormatException ex)
            {
                throw new UserServiceException("Invalid format", ex);
            }

            this.logger?.Info("Started new Master");
        }

        public bool IsLogging { get; }

        public void Add(User user)
        {
            if (ReferenceEquals(user, null))
            {
                var ex = new ArgumentNullException($"{nameof(user)} is null");
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            if (ReferenceEquals(user.FirstName, null) || ReferenceEquals(user.LastName, null))
            {
                var ex = new InvalidUserException($"{nameof(user)} isn't fully initialized");
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            if (this.storage.Contains(user))
            {
                var ex = new UserAlreadyExistException($"{nameof(user)} already exists");
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            rwLockSlim.EnterWriteLock();
            try
            {

                user.Identifier = this.generateIdentifierFunction();

                if (user.Identifier < 0)
                {
                    var ex = new InvalidIdentifierException($"{nameof(user.Identifier)} can't be under zero");
                    this.logger?.Error(ex, ex.Message);
                    throw ex;
                }

                if (storage.Select(usr => usr.Identifier).Contains(user.Identifier))
                {
                    var ex = new InvalidIdentifierException($"{nameof(user.Identifier)} '{user.Identifier}' already has been taken");
                    this.logger?.Error(ex, ex.Message);
                    throw ex;
                }

                this.logger?.Info($"Added {nameof(user)} with fields:\n ID:{user.Identifier}\n FirstName:{user.FirstName}\n LastName:{user.LastName}\n Age:{user.Age}");

                this.storage.Add(user);

            }
            finally
            {
                rwLockSlim.ExitWriteLock();
            }

            Notify(new Message { Action = Action.Add, User = user.Clone() });
        }

        public void Delete(User user)
        {
            if (ReferenceEquals(user, null))
            {
                var ex = new ArgumentNullException($"{nameof(user)} is null");
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            if (!this.storage.Contains(user))
            {
                var ex = new UserNotFoundException($"{nameof(user)} not found");
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            rwLockSlim.EnterWriteLock();
            try
            {
                this.logger?.Info($"Deleted {nameof(user)} with fields:\n ID:{user.Identifier}\n FirstName:{user.FirstName}\n LastName:{user.LastName}\n Age:{user.Age}");

                this.storage.Remove(user);
            }
            finally
            {
                rwLockSlim.ExitWriteLock();
            }

            Notify(new Message { Action = Action.Delete, User = user.Clone()});
        }

        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (ReferenceEquals(predicate, null))
            {
                var ex = new ArgumentNullException($"{nameof(predicate)} is null");
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            List<User> result;

            rwLockSlim.EnterReadLock();
            try
            {
                result = this.storage.Where(user => predicate(user)).ToList();

            }
            finally
            {
                rwLockSlim.ExitReadLock();
            }

            this.logger?.Info($"Requested {result.Count} users: Id's {string.Concat(" ", result.Select(user => user.Identifier))}");

            return result;
        }

        public void SaveCurrentState(ISaveState saver)
        {
            if (ReferenceEquals(saver, null))
            {
                var ex = new ArgumentNullException($"{nameof(saver)} is null");
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            try
            {
                rwLockSlim.EnterReadLock();
                try
                {
                    saver.Save(this.storage);
                }
                finally
                {
                    rwLockSlim.ExitReadLock();
                }

                this.logger?.Info($"Saved {this.storage.Count} users");
            }
            catch (IOException exception)
            {
                var ex = new UserServiceException("Can't save Users", exception);
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }
            catch (InvalidOperationException exception)
            {
                var ex = new UserServiceException("Can't save Users", exception);
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }
        }

        public void LoadPreviousSate(ISaveState saver)
        {
            if (ReferenceEquals(saver, null))
            {
                var ex = new ArgumentNullException($"{nameof(saver)} is null");
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            try
            {
                rwLockSlim.EnterReadLock();
                try
                {
                    this.storage = new HashSet<User>(saver.Load(), this.userEqualityComparer);
                }
                finally
                {
                    rwLockSlim.ExitReadLock();
                }

                this.logger?.Info($"Requested {this.storage.Count} users from storage");
            }
            catch (IOException exception)
            {
                var ex = new UserServiceException("Can't load Users", exception);
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }
            catch (InvalidOperationException exception)
            {
                var ex = new UserServiceException("Can't load Users", exception);
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            foreach (var user in storage)
            {
                Notify(new Message { Action = Action.Add, User = user.Clone() });
            }
        }
        private void Notify(Message message)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            foreach (var salveConfig in Configurator.GetSlaveConfig)
            {
                using (TcpClient client = new TcpClient(salveConfig.Address, salveConfig.Port))
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        formatter.Serialize(stream, message);
                    }
                }
            }
        }
    }
}
