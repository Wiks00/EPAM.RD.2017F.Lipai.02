using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServiceLibrary
{
    public class UserEqualityComparer : IEqualityComparer<User>
    {
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

        public int GetHashCode(User user)
            => user.FirstName.GetHashCode() + user.LastName.GetHashCode();
    }
}
