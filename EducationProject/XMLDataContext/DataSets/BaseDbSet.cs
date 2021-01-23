using EducationProject.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using XMLDataContext.Interfaces;

namespace XMLDataContext.DataSets
{
    public class BaseDbSet<T> : IDbSet<T> where T : BaseEntity
    {
        IXMLParser<T> _parser;

        private XDocument _document;

        private int _currentId;

        public BaseDbSet(IXMLParser<T> Parser, XDocument Document)
        {
            _parser = Parser;

            _document = Document;

            if (_document.Root.Elements(_parser.ElementName).Any() == true)
            {
                _currentId = _document.Root.Elements(_parser.ElementName)
                    .Select(e => Int32.Parse(e.Element("Id").Value)).Max() + 1;
            }
            else
            {
                _currentId = 1;
            }
        }

        private List<T> Created = new List<T>();

        private List<T> Updated = new List<T>();

        private List<T> Deleted = new List<T>();

        public int CurrentId => _currentId++;

        public void Create(T Entity)
        {
            Entity.Id = CurrentId;
            Created.Add(Entity);
        }

        public void Update(T Entity)
        {
            Update(Entity, e => e.Id == Entity.Id);
        }

        public void Update(T Entity, Predicate<T> Condition)
        {
            foreach (var element in Get(Condition))
            {
                var UpdateIndex = Created.FindIndex(e => e.Id == element.Id);

                if (UpdateIndex != -1)
                {
                    Created[UpdateIndex] = element;
                    continue;
                }

                UpdateIndex = Updated.FindIndex(e => e.Id == element.Id);

                if (UpdateIndex != -1)
                {
                    Updated[UpdateIndex] = Entity;
                    continue;
                }

                Updated.Add(Entity);
            }
        }

        public T Get(T Entity)
        {
            return Get(e => e.Id == Entity.Id).FirstOrDefault();
        }

        public T Get(int Id)
        {
            return Get(e => e.Id == Id).FirstOrDefault();
        }

        public IEnumerable<T> Get(Predicate<T> Condition)
        {
            List<T> foundElements = new List<T>();

            foundElements.AddRange(Updated.Where(e => Condition(e) == true));

            foundElements.AddRange(Created.Where(e => Condition(e) == true && foundElements.Any(d => d.Id == e.Id) == false));

            foundElements.AddRange(FindInLocalCollection(Condition).Where(e => foundElements.Any(d => d.Id == e.Id) == false));

            return foundElements; 
        }

        public void Delete(T Entity)
        {
            Delete(e => e.Id == Entity.Id);
        }

        public void Delete(int Id)
        {
            Delete(e => e.Id == Id);
        }

        public void Delete(Predicate<T> Condition)
        {
            var tttyy = Get(Condition);
            foreach (var element in Get(Condition))
            {
                var DeleteIndex = Created.FindIndex(e => e.Id == element.Id);

                if (DeleteIndex != -1)
                {
                    Created.RemoveAt(DeleteIndex);
                    continue;
                }

                DeleteIndex = Updated.FindIndex(e => e.Id == element.Id);

                if (DeleteIndex != -1)
                {
                    Updated.RemoveAt(DeleteIndex);
                    continue;
                }

                Deleted.Add(element);
            }
        }

        public IEnumerable<T> FindInLocalCollection(Predicate<T> Condition)
        {
            return _document.Root.Elements(_parser.ElementName)
                .Select(e => _parser.ParseToClass(e)).Where(e => Condition(e) == true);
        }

        private bool ExistInLocalCollection(Predicate<T> Condition)
        {
            return FindInLocalCollection(Condition).Any();
        }

        public void Save()
        {
            foreach (var element in _document.Root.Elements(_parser.ElementName).ToList())
            {
                int currentId = Int32.Parse(element.Element("Id").Value);

                if (Deleted.Any(e => e.Id == currentId))
                {
                    element.Remove();
                }

                int UpdateIndex = Updated.FindIndex(e => e.Id == currentId);

                if (UpdateIndex != -1)
                {
                    element.ReplaceWith(_parser.ParseToXElement(Updated[UpdateIndex]));
                }
            }

            foreach(var element in Created)
            {
                _document.Root.Add(_parser.ParseToXElement(element));
            }

            Created.Clear();
            Updated.Clear();
            Deleted.Clear();
        }
    }
}
