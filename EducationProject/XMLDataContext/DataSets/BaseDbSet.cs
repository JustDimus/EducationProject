using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using XMLDataContext.Interfaces;

namespace XMLDataContext.DataSets
{
    public class BaseDbSet<T> : IDbSet<T>
    {
        IXMLParser<T> _parser;

        private XDocument _document;

        public BaseDbSet(IXMLParser<T> Parser, XDocument Document)
        {
            _parser = Parser;
            _elements = new Dictionary<T, ElementState>();

            _document = Document;
        }

        private Dictionary<T, ElementState> _elements;

        public Dictionary<T, ElementState> Elements => _elements;

        public void Delete(T Entity)
        {
            throw new NotImplementedException();
        }

        public T Get(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Predicate<T> Condition)
        {
            throw new NotImplementedException();
        }

        public T Get(T Entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var t in _elements)
                yield return _parser.ParseToClass(null);
            yield break;
        }

        public void Insert(T Entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(T Entity)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
