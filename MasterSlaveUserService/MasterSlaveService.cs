using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MasterSlaveUserService.Interfaces;
using ServiceLibrary;
using ServiceLibrary.Config;
using ServiceLibrary.Interfaces;

namespace MasterSlaveUserService
{
    public class MasterSlaveService : IDisposable
    {
        private readonly List<AppDomain> domansList = new List<AppDomain>(); 
        private readonly List<ISlaveService> slaves = new List<ISlaveService>();
        private readonly MasterService master;
        public IUserService Service { get; set; }

        public MasterSlaveService(IUserService service)
        {
            Service = service;
            master = CreateMaster(service);
            for (int i = 0; i < Configurator.GetSlaveConfig; i++)
            {
                slaves.Add(CreateSlave(i,master));
            }
        }

        private MasterService CreateMaster(IUserService service)
        {
            AppDomainSetup appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Master")
            };

            AppDomain domain = AppDomain.CreateDomain
                ("Master", null, appDomainSetup);
            domansList.Add(domain);

            var master = (MasterService)domain.CreateInstanceAndUnwrap
                ("MasterSlaveUserService", typeof (MasterService).FullName, false, BindingFlags.CreateInstance, null, new object[] {service}, null,
                    null);

            return master;
        }

        private SlaveService CreateSlave(int id, IMasterService master)
        {
            AppDomainSetup appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Slave")
            };

            AppDomain domain = AppDomain.CreateDomain
                ($"Slave{id}", null, appDomainSetup);
            domansList.Add(domain);

            var slave = (SlaveService) domain.CreateInstanceAndUnwrap("MasterSlaveUserService", typeof (SlaveService).FullName );
            slave.Listen(master);
            return slave;
        }

        public void Add(User user) => master.Add(user);

        public void Delete(User user) => master.Delete(user);
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            int slave = slaves.Count == 1 ? 0 : new Random().Next(0, slaves.Count - 1);

            return slaves[slave].Search(predicate);
        }

        public void Dispose()
        {
            foreach (var appDomain in domansList)
            {
                AppDomain.Unload(appDomain);
            }
        }
    }
}
