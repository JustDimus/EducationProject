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

        public IEnumerable<AccountDBO> Get(Expression<Func<AccountDBO, bool>> condition)
        {
            return this.context.Accounts.Where(condition)
                .Include(a => a.AccountCourses)
                .ThenInclude(ac => ac.Course)
                .ThenInclude(c => c.CourseSkills)
                .ThenInclude(cs => cs.Skill)
                .Include(a => a.AccountCourses) //
                .ThenInclude(ac => ac.Course)
                .ThenInclude(c => c.CourseMaterials)
                .ThenInclude(cm => cm.Material)
                .ThenInclude(m => m.Video)
                .Include(a => a.AccountCourses)
                .ThenInclude(ac => ac.Course)
                .ThenInclude(c => c.CourseMaterials)
                .ThenInclude(cm => cm.Material)
                .ThenInclude(m => m.Article)
                .Include(a => a.AccountCourses)
                .ThenInclude(ac => ac.Course)
                .ThenInclude(c => c.CourseMaterials)
                .ThenInclude(cm => cm.Material)
                .ThenInclude(m => m.Book);  //
        }

        public AccountDBO Get(int id)
        {
            return this.context.Accounts.Find(id);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(AccountDBO entity)
        {
            this.context.Accounts.Update(entity);
        }
    }
}
