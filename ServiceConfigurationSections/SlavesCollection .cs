using System.Collections.Generic;
using System.Configuration;

namespace ServiceConfiguration
{
    [ConfigurationCollection(typeof(SlaveElement), AddItemName = "slave")]
    public class SlavesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SlaveElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SlaveElement)element).Name;
        }

        public SlaveElement this[int idx] => (SlaveElement)BaseGet(idx);
    }
}
