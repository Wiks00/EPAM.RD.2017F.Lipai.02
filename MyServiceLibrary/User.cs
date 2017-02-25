using System;

namespace ServiceLibrary
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class User
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
    }
}
