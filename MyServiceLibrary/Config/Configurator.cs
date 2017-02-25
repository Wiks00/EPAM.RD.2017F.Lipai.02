using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Config
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
                
                return bool.Parse(Section.LogItems[0].IsEnable); 
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

                return Section.DumpItems[0].FileName;
            }
        }

        public static int GetSlaveConfig
        {
            get
            {
                if (ReferenceEquals(Section, null))
                {
                    throw new ArgumentNullException($"{nameof(Section)} is null");
                }

                return int.Parse(Section.ServiceItems[0].Amount);
            }
        }
    }
}
