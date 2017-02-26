using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterSlaveUserService;
using ServiceLibrary;
using ServiceLibrary.Config;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new UserService();

            service.GetDump(new XmlDumper(Configurator.GetDumpConfig));

            User[] users = 
            {
                new User { FirstName = "Ilya1", LastName = "Lipai1", Age = 20 },
                new User { FirstName = "And1", LastName = "I'm1", Age = 218 },
                new User { FirstName = "Another1", LastName = "Ver1y", Age = 20 },
                new User { FirstName = "User1", LastName = "Imaginative1", Age = 100500 }
            };

            var a = new MasterSlaveService(new UserService());

            foreach (var user in users)
            {
                a.Add(user);
            }

            a.Delete(users[2]);

            var testSearch = a.Search(user => user.Age == 20);

            Console.ReadKey();
        }
    }
}
