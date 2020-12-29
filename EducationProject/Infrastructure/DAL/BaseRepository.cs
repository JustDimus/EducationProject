using DomainCore.BLL;
using DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Infrastructure.DAL
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private string filePath { get; set; }

        protected abstract Func<XElement, T> convertFunc { get; }

        public BaseRepository(string FilePath)
        {
            filePath = FilePath;
        }

        public IEnumerable<T> Get(Predicate<int> Condition)
        {
            throw new NotImplementedException();
        }

        public void Delete(T Entity)
        {
            throw new NotImplementedException();
        }

        public T Get(int Id)
        {
            throw new NotImplementedException();
        }

        public T Get(T Entity)
        {
            throw new NotImplementedException();
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
    }
}
