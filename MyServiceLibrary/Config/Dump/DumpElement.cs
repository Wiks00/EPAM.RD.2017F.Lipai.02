using System.Configuration;

namespace ServiceLibrary.Config.Dump
{
    public class DumpElement : ConfigurationElement
    {
        [ConfigurationProperty("fileName", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string FileName => (string)base["fileName"];
    }
}
