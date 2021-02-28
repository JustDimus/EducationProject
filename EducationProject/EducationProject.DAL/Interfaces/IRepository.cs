using EducationProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EducationProject.DAL.Interfaces
{
    public interface IRepository<TEntity>
    {
        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        int Count(Expression<Func<TEntity, bool>> condition);

        TEntity Get(Expression<Func<TEntity, bool>> condition);

        TResult Get<TResult>(Expression<Func<TEntity, bool>> condition, 
            Expression<Func<TEntity, TResult>> selector);

        IEnumerable<TEntity> GetPage(Expression<Func<TEntity, bool>> condition, 
            int pageNumber, int pageSize);

        IEnumerable<TResult> GetPage<TResult>(Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TResult>> selector, int pageNumber, int pageSize);

        //TOut Get<TOut>(int id, Expression<Func<T, TOut>> selector);

        void Delete(Expression<Func<TEntity, bool>> condition);

        void Delete(int id);

        bool Any(Expression<Func<TEntity, bool>> condition);

        void Save();
    }
}
