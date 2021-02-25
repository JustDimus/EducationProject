using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
using EducationProject.DAL.Mappings;
using EducationProject.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.DAL.EF.Mappings
{
    public class AccountMappingEF : IMapping<AccountDBO>
    {
        private EducationProjectDbContext context;

        public AccountMappingEF(EducationProjectDbContext dbContext)
        {
            this.context = dbContext;
        }

        public void Create(AccountDBO entity)
        {
            this.context.Accounts.Add(entity);
        }

        public void Delete(AccountDBO entity)
        {
            this.context.Accounts.Remove(entity);
        }

        public void Delete(int id)
        {
            this.context.Accounts.Remove(this.context.Accounts.Find(id));
        }

        public void Delete(Expression<Func<AccountDBO, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public void Delete(Predicate<AccountDBO> condition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TOut> Get<TOut>(Expression<Func<AccountDBO, bool>> condition, 
            Expression<Func<AccountDBO, TOut>> selector, int pageNumber, int pageSize)
        {
            int currentPage = pageNumber * pageSize;

            return this.context.Accounts.Where(condition).Select(selector).Skip(pageNumber * pageSize).Take(pageSize);
        }

        public IEnumerable<AccountDBO> Get<TOut, TJoin>(Expression<Func<AccountDBO, bool>> condition, 
            int pageNumber = 0, int pageSize = 30, 
            params Expression<Func<AccountDBO, object>>[] includes)
        {
            int startPage = pageNumber * pageSize;

            var result = this.context.Accounts;

            foreach(var include in includes)
            {
                result.Include(include);
            }

            return result.Where(condition).Skip(startPage).Take(pageSize);
        }

        public TOut Get<TOut>(int id, Expression<Func<AccountDBO, TOut>> selector)
        {
            throw new NotImplementedException();
            //return this.context.Accounts.Find(id);
        }

        public IEnumerable<AccountDBO> Get(Predicate<AccountDBO> condition)
        {
            throw new NotImplementedException();
        }

        public bool Any(Expression<Func<AccountDBO, bool>> condition)
        {
            return this.context.Accounts.Any(condition);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(AccountDBO entity)
        {
            this.context.Accounts.Update(entity);
        }

        public AccountDBO Get(Expression<Func<AccountDBO, bool>> condition)
        {
            return this.context.Accounts.Where(condition).FirstOrDefault();
        }

        public TResult Get<TResult>(Expression<Func<AccountDBO, bool>> condition, Expression<Func<AccountDBO, TResult>> selector)
        {
            return this.context.Accounts.Where(condition).Select(selector).FirstOrDefault();
        }

        public IEnumerable<AccountDBO> GetPage(Expression<Func<AccountDBO, bool>> condition, int pageNumber, int pageSize)
        {
            int skipRows = (pageNumber - 1) * pageSize;

            return this.context.Accounts.Where(condition).Skip(skipRows).Take(pageSize);
        }

        public IEnumerable<TResult> GetPage<TResult>(Expression<Func<AccountDBO, bool>> condition, Expression<Func<AccountDBO, TResult>> selector, int pageNumber, int pageSize)
        {
            int skipRows = (pageNumber - 1) * pageSize;

            return this.context.Accounts.Where(condition).Select(selector).Skip(skipRows).Take(pageSize);
        }
    }
}
