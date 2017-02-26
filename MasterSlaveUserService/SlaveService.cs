using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterSlaveUserService.Interfaces;
using ServiceLibrary;
using ServiceLibrary.Exceptions;

namespace MasterSlaveUserService
{
    public class SlaveService : MarshalByRefObject, ISlaveService
    {
        private readonly HashSet<User> localStorage;

        public SlaveService()
        {
            localStorage = new HashSet<User>(User.GetComparer);
        }

        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException($"{nameof(predicate)} is null");
            }

            var result = this.localStorage.Where(user => predicate(user)).Select(u => u.Clone()).ToList();

            if (result.Count < 0)
            {
                throw new UserNotFoundException("User(s) not found");
            }

            return result;
        }

        public void Listen(IMasterService master)
        {
            master.NotificationEvent += Synchronize;
        }

        public void Synchronize(object sender, ActionEventArgs action)
        {
            switch (action.Action)
            {
                case Action.Add:
                    localStorage.Add(action.User);
                    break;
                case Action.Delete:
                    localStorage.Remove(action.User);
                    break;
            }
        }
    }
}
