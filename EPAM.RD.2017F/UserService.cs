using EPAM.RD._2017F.Entities;
using EPAM.RD._2017F.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using EPAM.RD._2017F.Interfaces;

namespace EPAM.RD._2017F
{
    internal class UserService : IService<User>, IXmlSerializable
    {
        private ICollection<User> usersList;
        private Func<int> idGenerator;
        private IEqualityComparer<User> comparer;

        public event EventHandler<UserServiceAddEventArgs> AddUser = delegate { };
        public event EventHandler<UserServiceRemoveEventArgs> RemoveUser = delegate { };

        /// <summary>
        /// Create instance of UserService
        /// </summary>
        /// <param name="idGenerator">Predicate for generating identities</param>
        /// <param name="equalityComparer">IEqualityComparer implementation</param>
        public UserService(Func<int> idGenerator, bool isMaster = false, IEqualityComparer<User> equalityComparer = null)
        {
            this.idGenerator = idGenerator;
            comparer = equalityComparer ?? EqualityComparer<User>.Default;
            usersList = new List<User>();

            IsMaster = isMaster;
        }

        public bool IsMaster { get; private set; }

        /// <summary>
        /// IEqualityComparer implementation used for determine equality of users
        /// </summary>
        public IEqualityComparer<User> Comparer
        {
            get
            {
                return comparer ?? EqualityComparer<User>.Default;
            }
            set
            {
                if (value != null)
                    comparer = value;
            }
        }

        /// <summary>
        /// Add user to storage
        /// </summary>
        /// <param name="e">User to add</param>
        public void Add(User e)
        {
            if (!IsMaster)
                throw new ServiceNotInMasterModeException();

            if (e == null)
                throw new ArgumentNullException();

            if (e.FirstName == null || e.LastName == null)
                throw new ArgumentNullException(nameof(e));

            e.Id = idGenerator.Invoke();
            if (usersList.Contains(e, comparer))
                throw new ExistingUserException();
            usersList.Add(e);
        }

        /// <summary>
        /// Delete user from storage
        /// </summary>
        /// <param name="e">User to delete</param>
        public void Delete(User e)
        {
            if (!IsMaster)
                throw new ServiceNotInMasterModeException();

            if (e == null)
                throw new ArgumentNullException();

            var removingUser = usersList.FirstOrDefault(u => comparer.Equals(u, e));

            if (removingUser == null)
                throw new UserNotExists();

            usersList.Remove(removingUser);
        }

        /// <summary>
        /// Find user by predicate
        /// </summary>
        /// <param name="predicate">Search expression</param>
        /// <returns>Enumeration with finded results</returns>
        public IEnumerable<User> Search(Func<User, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException();

            return usersList.Where(predicate);
        }

        //public void SaveState(string fileName)
        //{
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserService));
        //    string xml = "";

        //    using (var stringWriter = new StringWriter())
        //    {
        //        using (XmlWriter writer = XmlWriter.Create(stringWriter))
        //        {
        //            xmlSerializer.Serialize(writer, this);
        //            xml = stringWriter.ToString();
        //        }
        //    }

        //    File.WriteAllText(fileName, xml);
        //}

        
        public static UserService RestoreState(string fileName, IStorage<UserService> storage)
        {
            return storage.RestoreState(fileName);
        }

        public void SaveState(string fileName, IStorage<UserService> storage)
        {
            if (!IsMaster)
                throw new ServiceNotInMasterModeException();

            storage.SaveState(fileName, this);
        }

        protected virtual void OnUserAdded(UserServiceAddEventArgs e)
        {
            EventHandler<UserServiceAddEventArgs> temp = AddUser;
            temp?.Invoke(this, e);
        }

        protected virtual void OnUserRemoved(UserServiceRemoveEventArgs e)
        {
            EventHandler<UserServiceRemoveEventArgs> temp = RemoveUser;
            temp?.Invoke(this, e);
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (usersList.Count > 0)
                usersList.Clear();

            using (reader)
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        User user = new User {
                            Id = int.Parse(reader.GetAttribute("Id")),
                            FirstName = reader.GetAttribute("FirstName"),
                            LastName = reader.GetAttribute("LastName"),
                            Age = int.Parse(reader.GetAttribute("Age"))
                        };

                        usersList.Add(user);
                    }
                }
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            using (writer)
            {
                foreach (User user in usersList)
                {
                    writer.WriteStartElement("user");
                    writer.WriteAttributeString("Id", user.Id.ToString());
                    writer.WriteAttributeString("FirstName", user.FirstName);
                    writer.WriteAttributeString("LastName", user.LastName);
                    writer.WriteAttributeString("Age", user.Age.ToString());
                    writer.WriteEndElement();
                }
            }
        }
    }
}
