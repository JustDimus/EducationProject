using EducationProject.Core;
using EducationProject.DAL;
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

        public void Create(T Entity)
        {
            Elements.Create(Entity);
        }

        public void Delete(T Entity)
        {
            Elements.Delete(Entity);
        }

        public void Delete(Predicate<T> Condition)
        {
            Elements.Delete(Condition);
        }

        public void Delete(int Id)
        {
            Elements.Delete(Id);
        }

        public IEnumerable<T> Get(Predicate<T> Condition)
        {
            return Elements.Get(Condition);
        }

        public T Get(int Id)
        {
            return Elements.Get(Id);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(T Entity)
        {
            Elements.Update(Entity);
        }

        public void Update(T Entity, Predicate<T> Condition)
        {
            Elements.Update(Entity, Condition);
        }
    }
}
