using System;

namespace MasterSlaveUserService.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAlreadyExistException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public UserAlreadyExistException()
        {          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public UserAlreadyExistException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UserAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
