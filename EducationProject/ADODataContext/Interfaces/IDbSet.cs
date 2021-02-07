using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ADODataContext.Interfaces
{
    public interface IDbSet<T> : IDbSet
    {
        void Create(T entity);

        IEnumerable<T> Get();

        IEnumerable<T> Get(Expression<Func<T, bool>> condition);

        T Get(int id);

        T Get(T entity);

        void Update(T entity);

        void Update(T entity, Expression<Func<T, bool>> condition);

        void Delete(T entity);

        void Delete(int id);

        void Delete(Expression<Func<T, bool>> condition);

        int CurrentId { get; }
    }
}
