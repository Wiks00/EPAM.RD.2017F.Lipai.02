using System;
using System.Collections.Generic;
using ServiceLibrary.Exceptions;

namespace ServiceLibrary.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="user">adding user</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidUserException"></exception>
        /// <exception cref="UserAlreadyExistException"></exception>
        /// <exception cref="InvalidIdentifierException"></exception>
        void Add(User user);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="user">deleting user</param>
        /// <exception cref="ArgumentNullException"></exception>
        void Delete(User user);

        /// <summary>
        /// Serch user
        /// </summary>
        /// <param name="predicate">serching predicate</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>user enumiration</returns>
        IEnumerable<User> Search(Predicate<User> predicate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dumper"></param>
        void Dump(IDump dumper);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dumper"></param>
        void GetDump(IDump dumper);

    }
}
