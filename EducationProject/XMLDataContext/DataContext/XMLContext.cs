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

        private string _fileName;

        private XDocument _document;

        private string filePath { get; set; }

        public XMLContext()
        {
            _fileName = ConfigurationManager.AppSettings.Get("XMLFile");
            _document = new FileCheckers.FileChecker(_fileName).Get();
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
            _users?.Save();

            _document.Save(_fileName);
        }
    }
}
