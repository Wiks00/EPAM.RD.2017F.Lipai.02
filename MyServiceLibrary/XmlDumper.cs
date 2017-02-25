using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ServiceLibrary.Interfaces;

namespace ServiceLibrary
{
    public class XmlDumper : IDump
    {
        private readonly string path; 

        public XmlDumper(string fileName)
        {
            path = Path.Combine(Environment.CurrentDirectory, fileName);
        }

        public void Save(IEnumerable<User> storageContent)
        {
            if (ReferenceEquals(storageContent, null))
            {
                throw new ArgumentNullException($"{nameof(storageContent)} is null");
            }

            var formatter = new XmlSerializer(typeof(HashSet<User>));

            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fileStream, storageContent);
            }
        }

        public IEnumerable<User> Load()
        {
            var formatter = new XmlSerializer(typeof(List<User>));
            IEnumerable<User> users;

            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                users = (IEnumerable<User>)formatter.Deserialize(fileStream);
            }

            return users;
        }
    }
}
