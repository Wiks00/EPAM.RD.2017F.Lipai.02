using System.Configuration;

namespace ServiceLibrary.Config.Log
{
    [ConfigurationCollection(typeof(LogElement), AddItemName = "logger")]
    public class LogCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LogElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LogElement)element).IsEnable;
        }

        public LogElement this[int idx] => (LogElement)BaseGet(idx);
    }
}
