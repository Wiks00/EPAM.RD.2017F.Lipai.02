using System;
using System.Collections.Generic;

namespace ServiceLibrary
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class User : ICloneable
    {
        /// <summary>
        /// unique identifier
        /// </summary>
        public long? Identifier { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Age { get; set; }

        public static IEqualityComparer<User> GetComparer => new UserEqualityComparer(); 

        public User Clone()
          => new User
          {
              Age = this.Age,
              FirstName = this.FirstName,
              Identifier = this.Identifier,
              LastName = this.LastName
          };

        object ICloneable.Clone()
            => Clone();

        public override string ToString()
        {
            return $"{FirstName} {LastName} {Age}";
        }
    }
}
