using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLibrary;

namespace MasterSlaveUserService.Interfaces
{
    public interface ISlaveService
    {
        IEnumerable<User> Search(Predicate<User> predicate);
        void Listen();
    }
}
