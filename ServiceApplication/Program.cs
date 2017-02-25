using System;
using System.ComponentModel;
using MyServiceLibrary;

namespace ServiceApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new UserService();
            var bs  = new ByteConverter();
            var a = bs.ConvertFrom("stsdtst");

            Console.ReadKey();
            // 1. Add a new user to the storage.
            // 2. Remove an user from the storage.
            // 3. Search for an user by the first name.
            // 4. Search for an user by the last name.
        }
    }
}
