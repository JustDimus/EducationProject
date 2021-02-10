using EducationProject.Core.DAL.EF;
using EducationProject.DAL.Mappings;
using EducationProject.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DAL.EF.Mappings
{
    public class CourseMappingEF : IMapping<CourseDBO>
    {
        private EducationProjectDbContext context;

        public CourseMappingEF(EducationProjectDbContext dbContext)
        {
            context = dbContext;
        }

        public void Create(CourseDBO entity)
        {
            this.context.Courses.Add(entity);
        }

        public void Delete(CourseDBO entity)
        {
            this.context.Remove(entity);
        }

        public void Delete(Expression<Func<CourseDBO, bool>> condition)
        {
            this.context.Courses
                .RemoveRange(this.context.Courses.Where(condition));
        }

        public void Delete(int id)
        {
            this.context.Courses
                .Remove(this.context.Courses.Find(id));
        }

        public IEnumerable<CourseDBO> Get(Expression<Func<CourseDBO, bool>> condition)
        {
            return this.context.Courses.Where(condition);
        }

        public CourseDBO Get(int id)
        {
            return this.context.Courses.Find(id);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(CourseDBO entity)
        {
            this.context.Courses.Update(entity);
        }
    }
}
