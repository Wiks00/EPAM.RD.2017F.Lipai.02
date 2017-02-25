using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ServiceLibrary;
using ServiceLibrary.Exceptions;
using ServiceLibrary.Interfaces;

namespace MyServiceLibrary.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public void Add_InvaliddUser_InvalidUserExceptionThrow()
        {
            IUserService service = new UserService();
            User user = new User();

            Assert.Throws<InvalidUserException>(() => service.Add(user));
        }

        [Test]
        public void Add_NullUser_InvalidUserExceptionThrow()
        {
            IUserService service = new UserService();

            Assert.Throws<ArgumentNullException>(() => service.Add(null));
        }

        [Test]
        public void Add_EqualUser_UserAlreadyExistExceptionThrow()
        {
            IUserService service = new UserService();
            User user1 = new User
            {
                FirstName = "Ilya",
                LastName = "Lipai",
                Age = 20
            };

            User user2 = new User
            {
                FirstName = "Ilya",
                LastName = "Lipai",
                Age = 20
            };

            Assert.Throws<UserAlreadyExistException>(() =>
            {
                service.Add(user1);
                service.Add(user2);
            });
        }

        [Test]
        public void Add_ValidUser_AddToStroge()
        {
            IUserService service = new UserService();
            User user = new User
            {
                FirstName = "Ilya",
                LastName = "Lipai",
                Age = 20
            };

            Assert.DoesNotThrow(() => service.Add(user));
        }

        [Test]
        public void Delete_ExistingUser_DeleteFromStorage()
        {
            IUserService service = new UserService();
            User user = new User
            {
                FirstName = "Ilya",
                LastName = "Lipai",
                Age = 20
            };

            Assert.DoesNotThrow(() =>
                {
                    service.Add(user);
                    service.Delete(user);
                });
        }

        [Test]
        public void Delete_NullUser_ExceptionThrown()
        {
            IUserService service = new UserService();

            Assert.Throws<ArgumentNullException>(() => service.Delete(null));
        }

        [Test]
        public void Delete_NotExistingUser_NothingHappend()
        {
            IUserService service = new UserService(() => +1);
            User user = new User
            {
                FirstName = "Ilya",
                LastName = "Lipai",
                Age = 20
            };

            Assert.DoesNotThrow(() => service.Delete(user));
        }

        [Test]
        public void Search_NullPredicate_NothingHappend()
        {
            IUserService service = new UserService();

            Assert.Throws<ArgumentNullException>(() => service.Search(null));
        }

        [Test]
        public void Search_SearchByAge_UserEnumeration()
        {
            IUserService service = new UserService();
            User[] users =
            {
                new User { FirstName = "Ilya", LastName = "Lipai", Age = 20 },
                new User { FirstName = "And", LastName = "I'm", Age = 218 },
                new User { FirstName = "Another", LastName = "Very", Age = 20 },
                new User { FirstName = "User", LastName = "Imaginative", Age = 100500 }
            };

            foreach (var user in users)
            {
                service.Add(user);
            }

            IEnumerable<User> result = service.Search(user => user.Age == 20);

            User[] expected =
            {
                users[0],
                users[2]
            };

            CollectionAssert.AreEqual(result, expected);
        }
    }
}
