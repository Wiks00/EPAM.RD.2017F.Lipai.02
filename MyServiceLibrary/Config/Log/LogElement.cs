using System;
using System.Configuration;

namespace ServiceLibrary.Config.Log
{
    public class LogElement : ConfigurationElement
    {
        [ConfigurationProperty("enable", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string IsEnable => (string)base["enable"];
    }
}
