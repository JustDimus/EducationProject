using EducationProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity: class
    {
        private DbContext context;

        public BaseRepository(DbContext dbContext)
        {
            context = dbContext;
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition)
        {
            return this.context.Set<TEntity>().AnyAsync(condition);
        }

        public Task<bool> ContainsAsync(TEntity entity)
        {
            return this.context.Set<TEntity>().ContainsAsync(entity);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> condition)
        {
            return this.context.Set<TEntity>().CountAsync(condition);
        }

        public Task CreateAsync(TEntity entity)
        {
            return this.context.Set<TEntity>().AddAsync(entity).AsTask();
        }

        public Task DeleteAsync(TEntity entity)
        {
            return Task.Run(() =>
                this.context.Set<TEntity>().Remove(entity));
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> condition)
        {
            return Task.Run(() =>
                this.context.Set<TEntity>().RemoveRange(this.context.Set<TEntity>().Where(condition)));
        }

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> condition)
        {
            return this.context.Set<TEntity>().Where(condition).FirstOrDefaultAsync();
        }

        public Task<TResult> GetAsync<TResult>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TResult>> selector)
        {
            return this.context.Set<TEntity>().Where(condition).Select(selector).FirstOrDefaultAsync();
        }

        public Task<IEnumerable<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>> condition, int pageNumber, int pageSize)
        {
            int skipRows = pageNumber * pageSize;

            return Task.Run<IEnumerable<TEntity>>(() => 
                this.context.Set<TEntity>().Where(condition).Skip(skipRows).Take(pageSize));
        }

        public Task<IEnumerable<TResult>> GetPageAsync<TResult>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TResult>> selector, int pageNumber, int pageSize)
        {
            int skipRows = pageNumber * pageSize;

            return Task.Run<IEnumerable<TResult>>(() =>
                this.context.Set<TEntity>().Where(condition).Select(selector).Skip(skipRows).Take(pageSize));
        }

        public Task SaveAsync()
        {
            return this.context.SaveChangesAsync();
        }

        public Task UpdateAsync(TEntity entity)
        {
            return Task.Run(() => 
                this.context.Set<TEntity>().Update(entity));
        }
    }
}
