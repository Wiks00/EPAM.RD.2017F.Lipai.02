using System.Configuration;

namespace ServiceConfiguration
{
    public class DumpElement : ConfigurationElement
    {
        [ConfigurationProperty("fileName", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string FileName => (string)base["fileName"];
    }
}
