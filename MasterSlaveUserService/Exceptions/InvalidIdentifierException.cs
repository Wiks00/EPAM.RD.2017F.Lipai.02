using System;

namespace MasterSlaveUserService.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class InvalidIdentifierException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public InvalidIdentifierException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidIdentifierException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidIdentifierException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
