using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyServiceLibrary.Exceptions;

namespace MyServiceLibrary
{
    public class UserService : IUserService
    {
        private readonly Func<long> generateIdentifierFunction;
        private readonly ICollection<User> storage;

        public UserService() : this(null, null)
        {
        }

        public UserService(IEqualityComparer<User> userEqualityComparer) : this(null, userEqualityComparer)
        {
        }

        public UserService(Func<long> generateIdentifierFunction):this(generateIdentifierFunction,null)
        {
        }

        public UserService(Func<long> generateIdentifierFunction, IEqualityComparer<User> userEqualityComparer)
        {
            if (ReferenceEquals(generateIdentifierFunction, null))
            {
                long counter = 0;
                this.generateIdentifierFunction = () => counter++;
            }
            else
            {
                this.generateIdentifierFunction = generateIdentifierFunction;
            }

            this.storage = new HashSet<User>(userEqualityComparer ?? new UserEqualityComparer());
        }

        public void Add(User user)
        {
            if (ReferenceEquals(user, null))
            {
                throw new ArgumentNullException($"{nameof(user)} is null");
            }

            if (ReferenceEquals(user.FirstName, null) || ReferenceEquals(user.LastName, null))
            {
                throw new InvalidUserException($"{nameof(user)} isn't fully initialized");
            }

            if (this.storage.Contains(user))
            {
                throw new UserAlreadyExistException($"{nameof(user)} already exists");
            }

            user.Identifier = this.generateIdentifierFunction();
            this.storage.Add(user);
        }

        public void Delete(User user)
        {
            if (ReferenceEquals(user, null))
            {
                throw new ArgumentNullException($"{nameof(user)} is null");
            }

            this.storage.Remove(user);
        }

        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException($"{nameof(predicate)} is null");
            }

            return this.storage.Where(user => predicate(user));
        }
    }
}
