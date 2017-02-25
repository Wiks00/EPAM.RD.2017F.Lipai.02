using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Config.Service
{
    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("amount", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Amount => (string)base["amount"];
    }
}
