using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Logger;
using ServiceConfiguration;
using ServiceLibrary.Exceptions;
using ServiceLibrary.Interfaces;

namespace ServiceLibrary
{
    /// <summary>
    /// Service that provide Add,Delete and Search operations on User entity in storage
    /// </summary>
    public class UserService : MarshalByRefObject, IUserService
    {
        private readonly Func<long?> generateIdentifierFunction;
        private readonly IEqualityComparer<User> userEqualityComparer;
        private readonly ILogger logger;

        private ICollection<User> storage;

        /// <summary>
        /// Defult Constructor. Generate id by increment and compare by first and last name
        /// </summary>
        public UserService() : this(null, null)
        {
        }

        /// <summary>
        /// Generate user id by increment
        /// </summary>
        /// <param name="userEqualityComparer">user comparer</param>
        public UserService(IEqualityComparer<User> userEqualityComparer) : this(null, userEqualityComparer)
        {
        }

        /// <summary>
        /// Compare user by first and last name
        /// </summary>
        /// <param name="generateIdentifierFunction">generate identifier function</param>
        public UserService(Func<long?> generateIdentifierFunction) : this(generateIdentifierFunction, null)
        {
        }

        /// <summary>
        /// Construc service by your self
        /// </summary>
        /// <param name="generateIdentifierFunction">generate identifier function</param>
        /// <param name="userEqualityComparer">user comparer</param>
        /// <param name="logger">logger (if logging is enable)</param>
        public UserService(Func<long?> generateIdentifierFunction, 
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

            GetIdGenerator = this.generateIdentifierFunction;

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
        }

        public bool IsLogging { get; }

        public Func<long?> GetIdGenerator { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void Add(User user)
        {
            if (ReferenceEquals(user, null))
            {
                var ex = new ArgumentNullException($"{nameof(user)} is null");
                this.logger?.Error(ex,ex.Message);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void Delete(User user)
        {
            if (ReferenceEquals(user, null))
            {
                var ex = new ArgumentNullException($"{nameof(user)} is null");
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            this.logger?.Info($"Deleted {nameof(user)} with fields:\n ID:{user.Identifier}\n FirstName:{user.FirstName}\n LastName:{user.LastName}\n Age:{user.Age}");

            this.storage.Remove(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (ReferenceEquals(predicate, null))
            {
                var ex = new ArgumentNullException($"{nameof(predicate)} is null");
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            var result = this.storage.Where(user => predicate(user));

            if (result.Count() < 0)
            {
                var ex = new UserNotFoundException("User(s) not found");
                this.logger?.Error(ex, ex.Message);
                throw ex; 
            }

            this.logger?.Info($"Requested {result.Count()} users: Id's {string.Concat(" ",result.Select(user => user.Identifier))}");

            return result.ToList();
        }

        public void Dump(IDump dumper)
        {
            if (ReferenceEquals(dumper, null))
            {
                var ex = new ArgumentNullException($"{nameof(dumper)} is null");
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            try
            {
                dumper.Save(this.storage);
                this.logger?.Info($"Dumped {this.storage.Count} users");
            }
            catch (IOException exception)
            {
                var ex = new UserServiceException("Can't save Users", exception);
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }
            catch(InvalidOperationException exception)
            {
                var ex = new UserServiceException("Can't save Users", exception);
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dumper"></param>
        public void GetDump(IDump dumper)
        {
            if (ReferenceEquals(dumper, null))
            {
                var ex = new ArgumentNullException($"{nameof(dumper)} is null");
                this.logger?.Error(ex, ex.Message);
                throw ex;
            }

            try
            {
                this.storage = new HashSet<User>(dumper.Load(),this.userEqualityComparer);
                this.logger?.Info($"Requested {this.storage.Count} users from dump file");
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
        }
    }
}
