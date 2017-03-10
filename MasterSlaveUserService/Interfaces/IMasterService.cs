using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLibrary;

namespace MasterSlaveUserService.Interfaces
{
    public interface IMasterService
    {
        void Add(User user);
        void Delete(User user);
        IEnumerable<User> Search(Predicate<User> predicate);
        void SaveCurrentState(ISaveState saver);
        void LoadPreviousSate(ISaveState saver);
    }
}
