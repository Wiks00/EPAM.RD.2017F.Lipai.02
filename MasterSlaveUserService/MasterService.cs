using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MasterSlaveUserService.Interfaces;
using ServiceLibrary;
using ServiceLibrary.Interfaces;

namespace MasterSlaveUserService
{
    public class MasterService : MarshalByRefObject, IMasterService
    {
        private readonly IUserService service;
        public event EventHandler<ActionEventArgs> NotificationEvent;

        public MasterService(IUserService service)
        {
            this.service = service;
        }

        public void Add(User user)
        {
            service.Add(user);
            Notify(new ActionEventArgs { Action = Action.Add, User = user.Clone() });
        }

        public void Delete(User user)
        {
           service.Delete(user);
           Notify(new ActionEventArgs { Action = Action.Delete, User = user.Clone()});
        }

        public IEnumerable<User> Search(Predicate<User> predicate)
            => service.Search(predicate).Select(u => u.Clone()).ToList();
        

        protected virtual void SendMessage(object sender, ActionEventArgs e) => NotificationEvent?.Invoke(this, e);

        public void Notify(ActionEventArgs args)
        {
            SendMessage(this, args);
        }
    }
}
