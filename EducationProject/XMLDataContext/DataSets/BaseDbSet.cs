using DomainCore.Common;
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
            _elements = new Dictionary<int, (T, ElementState)>();

            _document = Document;
        }

        private Dictionary<int, (T, ElementState)> _elements;

        public Dictionary<int, (T, ElementState)> Elements => _elements;

        public void Delete(T Entity)
        {
            _elements[Entity.Id] = (Entity, ElementState.Deleted);
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
            return _document.Root.Elements(_parser.ElementName)
                .Select(e => _parser.ParseToClass(e)).Where(t => Condition(t));
        }

        public T Get(T Entity)
        {
            return Get(Entity.Id);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach(var i in _document.Root.Elements(_parser.ElementName))
            {
                yield return _parser.ParseToClass(i);
            }
            yield break;
        }

        public void Insert(T Entity)
        {
            if (Get(Entity) != null)
                throw new Exception("Current entity already exist!");

            _elements[Entity.Id] = (Entity, ElementState.Created);
        }

        public void Save()
        {
            foreach(var i in _document.Root.Elements(_parser.ElementName))
            {
                int currentId = Int32.Parse(i.Attribute("Id").Value);
                if (_elements.ContainsKey(currentId))
                {
                    switch(_elements[currentId].Item2)
                    {
                        case ElementState.Updated:
                            i.ReplaceWith(_parser.ParseToXElement(_elements[currentId].Item1));
                            break;
                        case ElementState.Deleted:
                            i.Remove();
                            break;
                    }
                }
            }

            foreach (var i in (from t in _elements where t.Value.Item2 == ElementState.Created select t))
            {
                _document.Root.Add(_parser.ParseToXElement(i.Value.Item1));
            }

            foreach(var i in _elements.Keys.ToList())
            {
                _elements[i] = (_elements[i].Item1, ElementState.NoAction);
            }
        }

        public void Update(T Entity)
        {
            if(_elements.ContainsKey(Entity.Id))
            {
                if (_elements[Entity.Id].Item2 == ElementState.Created)
                {
                    _elements[Entity.Id] = (Entity, ElementState.Created);
                }
                else
                {
                    _elements[Entity.Id] = (Entity, ElementState.Updated);
                }
                return;
            }

            if (Get(t => t.Id == Entity.Id).FirstOrDefault() != null)
            {
                _elements[Entity.Id] = (Entity, ElementState.Updated);
            }
            else
            {
                _elements[Entity.Id] = (Entity, ElementState.Created);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var i in _document.Root.Elements(_parser.ElementName))
            {
                yield return _parser.ParseToClass(i);
            }
            yield break;
        }

        private T TryFindInLocalCollection(int Id)
        {
            return _elements.Values.Where(p => p.Item1.Id == Id).Select(p => p.Item1).FirstOrDefault();
        } 
    }
}
