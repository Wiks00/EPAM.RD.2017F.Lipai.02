using System;

namespace ServiceLibrary.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    class UserServiceException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public UserServiceException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public UserServiceException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UserServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
