using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDataContext.Interfaces
{
    public interface IDbSet<T>: IDbSet
    {
        void Create(T entity);

        IEnumerable<T> Get(Predicate<T> condition);

        T Get(int id);

        T Get(T entity);

        void Update(T entity);

        void Update(T entity, Predicate<T> condition);

        void Delete(T entity);

        void Delete(int id);

        void Delete(Predicate<T> condition);

        int CurrentId { get; }
    }
}
