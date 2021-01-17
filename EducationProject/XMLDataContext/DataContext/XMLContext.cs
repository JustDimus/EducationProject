﻿using DomainCore.BLL;
using XMLDataContext.DataSets;
using XMLDataContext.Interfaces;
using System.Configuration;
using XMLDataContext.Parsers;
using System.Xml.Linq;
using DomainCore.AccountData;

namespace XMLDataContext.DataContext
{
    public class XMLContext
    {
        private IDbSet<Account> _users;

        private string _fileName;

        private XDocument _document;

        private string filePath { get; set; }

        public XMLContext()
        {
            _fileName = ConfigurationManager.AppSettings.Get("XMLFile");
            _document = new FileCheckers.FileChecker(_fileName).Get();
        }

        public IDbSet<Account> Accounts
        {
            get
            {
                if(_users == null)
                {
                    _users = new BaseDbSet<Account>(null/*new UserXMLParser()*/, _document);
                }
                return _users;
            }
        }

        public void Save()
        {
            Accounts.Save();

            _document.Save(_fileName);
        }
    }
}
