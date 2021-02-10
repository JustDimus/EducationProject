using EducationProject.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EducationProject.DAL.Mappings
{
    public interface IMapping<T>
    {
        void Create(T entity);

        IEnumerable<T> Get(Expression<Func<T, bool>> condition);

        T Get(int id);

        void Update(T entity);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> condition);

        void Delete(int id);

        void Save();
    }
}
