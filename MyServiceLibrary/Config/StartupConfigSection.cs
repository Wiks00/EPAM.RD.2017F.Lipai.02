using System.Configuration;
using ServiceLibrary.Config.Dump;
using ServiceLibrary.Config.Log;
using ServiceLibrary.Config.Service;

namespace ServiceLibrary.Config
{
    public class StartupConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("log")]
        public LogCollection LogItems => ((LogCollection)(base["log"]));

        [ConfigurationProperty("dump")]
        public DumpCollection DumpItems => ((DumpCollection)(base["dump"]));

        [ConfigurationProperty("service")]
        public ServiceCollection ServiceItems => ((ServiceCollection)(base["service"]));
    }
}
