using DomainCore.BLL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using XMLDataContext.Interfaces;

namespace XMLDataContext.DataSets
{
    public class BaseDbSet<T> : IDbSet<T> where T: BaseEntity
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
            _elements.Add(Entity, ElementState.Deleted);
        }

        public T Get(int Id)
        {
            T result = TryFindInLocalCollection(Id);

            if (result != null)
            {
                return result;
            }
            else
            {
                return Get(Entity => Entity.Id == Id).FirstOrDefault();
            }
        }

        public IEnumerable<T> Get(Predicate<T> Condition)
        {
            return (from element in _document.Root.Elements(_parser.ElementName) select _parser.ParseToClass(element)).Where(t => Condition(t));
        }

        public T Get(T Entity)
        {
            return Get(Entity.Id);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach(var i in (from element in _document.Root.Elements(_parser.ElementName) select element))
            {
                yield return _parser.ParseToClass(i);
            }
            yield break;
        }

        public void Insert(T Entity)
        {
            _elements[Entity] = ElementState.Created;
//            _elements.Add(Entity, ElementState.Created);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(T Entity)
        {
            _elements[Entity] = ElementState.Updated;
 //           if(_elements.ContainsKey(Entity))
//                _elements.Add(Entity, ElementState.Updated);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var i in (from element in _document.Root.Elements(_parser.ElementName) select element))
            {
                yield return _parser.ParseToClass(i);
            }
            yield break;
        }

        private T TryFindInLocalCollection(int Id)
        {
            return (from pair in _elements where pair.Key.Id == Id select pair.Key).FirstOrDefault();
        } 
    }
}
