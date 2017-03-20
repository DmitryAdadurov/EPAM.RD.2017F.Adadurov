using EPAM.RD._2017F.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EPAM.RD._2017F.Storages
{
    public class XmlStorage : IStorage<UserService>
    {
        public UserService RestoreState(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserService));
            FileStream file = new FileStream(fileName, FileMode.Open);
            return (UserService)serializer.Deserialize(file);
        }

        public void SaveState(string fileName, UserService obj)
        {
            using (XmlTextWriter writer = new XmlTextWriter(fileName, null))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UserService));
                serializer.Serialize(writer, obj);
            }
        }
    }
}
