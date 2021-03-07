using EducationProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.DAL.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> condition);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> condition);

        Task<TResult> GetAsync<TResult>(
            Expression<Func<TEntity, bool>> condition, 
            Expression<Func<TEntity, TResult>> selector);

        Task<IEnumerable<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>> condition, 
            int pageNumber, int pageSize);

        Task<IEnumerable<TResult>> GetPageAsync<TResult>(Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TResult>> selector, int pageNumber, int pageSize);

        Task DeleteAsync(Expression<Func<TEntity, bool>> condition);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition);

        Task<bool> ContainsAsync(TEntity entity);

        Task SaveAsync();
    }
}
