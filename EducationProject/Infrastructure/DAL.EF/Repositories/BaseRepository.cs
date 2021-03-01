using EducationProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.DAL.EF.Mappings
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity: class
    {
        private DbContext context;

        public BaseRepository(DbContext dbContext)
        {
            context = dbContext;
        }

        public bool Any(Expression<Func<TEntity, bool>> condition)
        {
            return this.context.Set<TEntity>().Any(condition);
        }

        public bool Contains(TEntity entity)
        {
            return this.context.Set<TEntity>().Contains(entity);
        }

        public int Count(Expression<Func<TEntity, bool>> condition)
        {
            return this.context.Set<TEntity>().Count(condition);
        }

        public void Create(TEntity entity)
        {
            this.context.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            this.context.Set<TEntity>().Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> condition)
        {
            this.context.Set<TEntity>().RemoveRange(this.context.Set<TEntity>().Where(condition));
        }

        public void Delete(int id)
        {

            this.context.Set<TEntity>().Remove(this.context.Set<TEntity>().Find(id));
        }

        public TEntity Get(Expression<Func<TEntity, bool>> condition)
        {
            return this.context.Set<TEntity>().Where(condition).FirstOrDefault();
        }

        public TResult Get<TResult>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TResult>> selector)
        {
            return this.context.Set<TEntity>().Where(condition).Select(selector).FirstOrDefault();
        }

        public IEnumerable<TEntity> GetPage(Expression<Func<TEntity, bool>> condition, int pageNumber, int pageSize)
        {
            int skipRows = pageNumber * pageSize;

            return this.context.Set<TEntity>().Where(condition).Skip(skipRows).Take(pageSize);
        }

        public IEnumerable<TResult> GetPage<TResult>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TResult>> selector, int pageNumber, int pageSize)
        {
            int skipRows = pageNumber * pageSize;

            return this.context.Set<TEntity>().Where(condition).Select(selector).Skip(skipRows).Take(pageSize);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            this.context.Set<TEntity>().Update(entity);
        }
    }
}
