using System.Configuration;

namespace ServiceLibrary.Config.Dump
{
    [ConfigurationCollection(typeof(DumpElement), AddItemName = "dumper")]
    public class DumpCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DumpElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DumpElement)element).FileName;
        }

        public DumpElement this[int idx] => (DumpElement)BaseGet(idx);
    }
}
