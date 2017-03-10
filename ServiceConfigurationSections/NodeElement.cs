using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceConfiguration
{
    public class NodeElement : ConfigurationElement
    {
        [ConfigurationProperty("mode", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Mode => (string)base["mode"];
    }
}
