using System.Collections.Generic;

namespace ServiceLibrary.Interfaces
{
    /// <summary>
    /// Interface that provide contract for user storage dump 
    /// </summary>
    public interface IDump
    {
        /// <summary>
        /// Dump data
        /// </summary>
        /// <param name="storageContent">storage content</param>
        void Save(IEnumerable<User> storageContent);

        /// <summary>
        /// Load dump data
        /// </summary>
        /// <returns>user enumeration</returns>
        IEnumerable<User> Load();
    }
}
