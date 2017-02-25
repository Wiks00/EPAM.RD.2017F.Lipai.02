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
        event EventHandler<ActionEventArgs> NotificationEvent;
        void Add(User user);
        void Delete(User user);
        void Notify(ActionEventArgs args);
    }
}
