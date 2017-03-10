using System;

namespace MasterSlaveUserService.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class InvalidUserException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public InvalidUserException()
        {           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidUserException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidUserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
