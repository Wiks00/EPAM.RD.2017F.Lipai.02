using System;
using System.Collections.Generic;

namespace ServiceLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class UserEqualityComparer : IEqualityComparer<User>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public bool Equals(User lhs, User rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
            {
                return false;
            }

            return string.Equals(lhs.FirstName, rhs.FirstName, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(lhs.LastName, rhs.LastName, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetHashCode(User user)
            => user.FirstName.GetHashCode() + user.LastName.GetHashCode();
    }
}
