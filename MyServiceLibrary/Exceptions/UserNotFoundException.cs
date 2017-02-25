using System;

namespace ServiceLibrary.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public UserNotFoundException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public UserNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
