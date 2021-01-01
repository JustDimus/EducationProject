using DomainCore.BLL;
using System;
using XMLDataContext.DataSets;
using XMLDataContext.Interfaces;
using System.Configuration;
using XMLDataContext.Parsers;
using System.Xml.Linq;

namespace XMLDataContext.DataContext
{
    public class XMLContext
    {
        private IDbSet<User> _users;

        private XDocument _document;

        private string filePath { get; set; }

        public XMLContext()
        {
            _document = new FileCheckers.FileChecker(ConfigurationManager.AppSettings.Get("XMLFile")).Get();
        }

        public IDbSet<User> Users
        {
            get
            {
                if(_users == null)
                {
                    _users = new BaseDbSet<User>(new UserXMLParser(), _document);
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
