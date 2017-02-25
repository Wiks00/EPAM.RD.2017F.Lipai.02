using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLibrary;

namespace MasterSlaveUserService
{
    [Serializable]
    public class ActionEventArgs
    {
        public Action Action { get; set;}

        public User User { get; set; }
    }

    public enum Action
    {
        Add,
        Delete,
        Search
    }
}
