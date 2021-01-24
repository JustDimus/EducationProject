using XMLDataContext.DataSets;
using XMLDataContext.Interfaces;
using System.Configuration;
using XMLDataContext.Parsers;
using System.Xml.Linq;
using EducationProject.Core.DAL;
using EducationProject.Core;
using System.Collections.Generic;
using System;

namespace XMLDataContext.DataContext
{
    public class XMLContext
    {
        private Dictionary<Type, IDbSet> _entites = new Dictionary<Type, IDbSet>();

        public IDbSet<T> Entity<T>() where T : BaseEntity
        {
            var tt = typeof(T);

            IDbSet res = null;

            if (_entites.TryGetValue(typeof(T), out res) == false)
            {
                res = new BaseDbSet<T>(new BaseXMLParser<T>(), _document);

                _entites.Add(typeof(T), res);
            }

            return (IDbSet<T>)res;
        }

        private string _fileName;

        private XDocument _document;

        private string filePath { get; set; }

        public XMLContext()
        {
            _fileName = ConfigurationManager.AppSettings.Get("XMLFile");
            _document = new FileCheckers.FileChecker(_fileName).Get();
        }

        public void Save()
        {
            foreach(var i in _entites)
            {
                i.Value.Save();
            }

            _document.Save(_fileName);
        }
    }
}
