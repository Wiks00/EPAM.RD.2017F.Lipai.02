using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MasterSlaveUserService.Interfaces;
using ServiceConfiguration;
using ServiceLibrary;
using ServiceLibrary.Interfaces;

namespace MasterSlaveUserService
{
    public class MasterSlaveService : IDisposable
    {
        private AppDomain domain;
        public dynamic Instance { get; }

        public MasterSlaveService()
        {
            switch (Configurator.GetNodeConfig.ToLower())
            {
                case "master":
                    Instance = CreateMaster();
                    break;
                case "slave":
                    foreach (var config in Configurator.GetSlaveConfig)
                    {
                        {
                            Instance = CreateSlave(Configurator.GetSlaveConfig.First());
                            Console.WriteLine(config.Address + ":" + config.Port);
                            break;
                        }                      
                    }                                       
                    break;                   
            }
        }

        private IMasterService CreateMaster()
        {
            AppDomainSetup appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Master")
            };

            AppDomain domain = AppDomain.CreateDomain
                ("Master", null, appDomainSetup);

            this.domain = domain;

            var master = (MasterService)domain.CreateInstanceAndUnwrap
                ("MasterSlaveUserService", typeof (MasterService).FullName);

            return master;
        }

        private ISlaveService CreateSlave(SlaveConfiguration config)
        {
            AppDomainSetup appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Slave")
            };

            AppDomain domain = AppDomain.CreateDomain
                ($"{config.Name}", null, appDomainSetup);

            this.domain = domain;

            var slave = (SlaveService) domain.CreateInstanceAndUnwrap("MasterSlaveUserService", typeof (SlaveService).FullName, 
                false, BindingFlags.CreateInstance, null, new object[] { config.Address, config.Port, null}, null, null);
            return slave;
        }

        public void Dispose()
        {
            AppDomain.Unload(this.domain);
        }
    }
}
