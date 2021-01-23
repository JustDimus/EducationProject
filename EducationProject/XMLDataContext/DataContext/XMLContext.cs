﻿using XMLDataContext.DataSets;
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
        private Dictionary<Type, BaseDbSet> _entites = new Dictionary<Type, BaseDbSet>();

        public IDbSet<T> Entity<T>() where T : BaseEntity
        {
            BaseDbSet res = null;

            if (_entites.TryGetValue(typeof(T), out res))
            {
                return (IDbSet<T>)res;
            }
            else
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
