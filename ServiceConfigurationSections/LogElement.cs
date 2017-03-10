using System.Configuration;

namespace ServiceConfiguration
{
    public class LogElement : ConfigurationElement
    {
        [ConfigurationProperty("enable", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string IsEnable => (string)base["enable"];
    }
}
