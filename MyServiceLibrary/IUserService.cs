using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyServiceLibrary
{
    public interface IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        void Add(User user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        void Delete(User user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<User> Search(Predicate<User> predicate);
    }
}
