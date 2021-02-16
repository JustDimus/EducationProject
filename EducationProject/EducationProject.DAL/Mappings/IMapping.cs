using EducationProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EducationProject.DAL.Mappings
{
    public interface IMapping<T>
    {
        void Create(T entity);

        IEnumerable<T> Get(Expression<Func<T, bool>> condition, int pageNumber = 0, int pageSize = 30);

        T Get(int id);

        void Update(T entity);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> condition);

        void Delete(int id);

        bool Any(Expression<Func<T, bool>> condition);

        void Save();
    }
}
