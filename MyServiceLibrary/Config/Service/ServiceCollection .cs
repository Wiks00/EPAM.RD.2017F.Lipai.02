using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLibrary.Config.Log;

namespace ServiceLibrary.Config.Service
{
    [ConfigurationCollection(typeof(ServiceElement), AddItemName = "slave")]
    public class ServiceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).Amount;
        }

        public ServiceElement this[int idx] => (ServiceElement)BaseGet(idx);
    }
}
