using DomainCore.BLL;
using System;
using XMLDataContext.DataSets;
using XMLDataContext.Interfaces;

namespace XMLDataContext.DataContext
{
    public class XMLContext
    {
        private IDbSet<User> _users;

        private string filePath { get; set; }

        public XMLContext(string FilePath)
        {
            filePath = FilePath;
        }

        public IDbSet<User> Users
        {
            get
            {
                if(_users == null)
                {
                    _users = new UserDbSet();
                }
                return _users;
            }
        }

        public void Save()
        {
            //TODO
        }
    }
}
