using DomainCore.BLL;
using DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using XMLDataContext.DataContext;
using XMLDataContext.Interfaces;

namespace Infrastructure.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected IDbSet<T> Elements;

        public BaseRepository(IDbSet<T> elements)
        {
            Elements = elements;
        }

        private void CheckElements()
        {
            if (Elements == null)
                throw new Exception();
        }

        public IEnumerable<T> Get(Predicate<T> Condition)
        {
            CheckElements();

            return Elements.Get(Condition);
        }

        public void Delete(T Entity)
        {
            CheckElements();

            Elements.Delete(Entity);
        }

        public T Get(int Id)
        {
            CheckElements();

            return Elements.Get(Id);
        }

        public T Get(T Entity)
        {
            CheckElements();

            return Elements.Get(Entity);
        }

        public void Insert(T Entity)
        {
            CheckElements();

            Elements.Insert(Entity);
        }

        public void Save()
        {
            CheckElements();

            Elements.Save();
        }

        public void Update(T Entity)
        {
            CheckElements();

            Elements.Update(Entity);
        }
    }
}
