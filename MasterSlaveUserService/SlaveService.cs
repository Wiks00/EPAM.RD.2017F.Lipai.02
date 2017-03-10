using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Logger;
using MasterSlaveUserService.Interfaces;
using ServiceLibrary;
using ServiceLibrary.Exceptions;
using UserNotFoundException = MasterSlaveUserService.Exceptions.UserNotFoundException;

namespace MasterSlaveUserService
{
    public class SlaveService : MarshalByRefObject, ISlaveService
    {
        private readonly HashSet<User> localStorage;
        private readonly ReaderWriterLockSlim rwLockSlim = new ReaderWriterLockSlim();
        private readonly ILogger logger;

        private readonly string ip;
        private readonly int port;

        public SlaveService(string ip, int port, ILogger logger)
        {
            if(string.IsNullOrEmpty(ip))
                throw new ArgumentException("Unvalid value",nameof(ip));

            if (port < 1000 || port > 9999)
                throw new ArgumentException("Unvalid value", nameof(ip));

            if(ReferenceEquals(logger,null))
                this.logger = NLogLogger.Logger;

            localStorage = new HashSet<User>(User.GetComparer);
            this.ip = ip;
            this.port = port;

            this.logger?.Info($"Started new Slave listening ip - {ip}:{port}");

            new Thread(this.Listen).Start();
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
                result = this.localStorage.Where(user => predicate(user)).ToList();
            }
            finally
            {
                rwLockSlim.ExitReadLock();
            }

            if (result.Count < 0)
            {
                throw new UserNotFoundException("User(s) not found");
            }

            this.logger?.Info($"Requested {result.Count} users: Id's {string.Concat(" ", result.Select(user => user.Identifier))}");

            return result;
        }

        private void Listen()
        {
            TcpListener server  = new TcpListener(IPAddress.Parse(ip), port); 
            try
            {
                
                server.Start();

                BinaryFormatter formatter = new BinaryFormatter();

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();

                    using (NetworkStream stream = client.GetStream())
                    {
                        Message message = (Message)formatter.Deserialize(stream);

                        Synchronize(message);
                    }

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                throw e;
            }
            finally
            {
                server.Stop();
            }
        }
        private void Synchronize(Message message)
        {
            rwLockSlim.EnterWriteLock();
            try
            {
                switch (message.Action)
                {
                    case Action.Add:
                        localStorage.Add(message.User);
                        break;
                    case Action.Delete:
                        localStorage.Remove(message.User);
                        break;
                }
            }
            finally
            {
                rwLockSlim.ExitWriteLock();
            }
        }
    }
}
