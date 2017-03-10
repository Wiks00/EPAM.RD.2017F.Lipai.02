using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace ServiceConfiguration
{
    public static class Configurator
    {
        private static readonly StartupConfigSection Section;

        static Configurator()
        {
            Section = (StartupConfigSection)ConfigurationManager.GetSection("settings");
        }

        public static bool GetLoggerConfig {
            get
            {
                if (ReferenceEquals(Section, null))
                {
                    throw new ArgumentNullException($"{nameof(Section)} is null");
                }
                
                return bool.Parse(Section.LogItem.IsEnable); 
            }
        }

        public static string  GetDumpConfig
        {
            get
            {
                if (ReferenceEquals(Section, null))
                {
                    throw new ArgumentNullException($"{nameof(Section)} is null");
                }

                return Section.DumpItem.FileName;
            }
        }


        public static string GetNodeConfig
        {
            get
            {
                if (ReferenceEquals(Section, null))
                {
                    throw new ArgumentNullException($"{nameof(Section)} is null");
                }

                return Section.NodeItem.Mode;
            }
        }

        public static IEnumerable<SlaveConfiguration> GetSlaveConfig
        {
            get
            {
                if (ReferenceEquals(Section, null))
                {
                    throw new ArgumentNullException($"{nameof(Section)} is null");
                }

                return (from SlaveElement slave in Section.ServiceItems
                       select new SlaveConfiguration
                       {
                           Address = slave.Ip,
                           Name = slave.Name,
                           Port = int.Parse(slave.Port)
                       }).ToList();
            }
        }
    }
}
