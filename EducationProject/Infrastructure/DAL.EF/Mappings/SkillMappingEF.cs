﻿using EducationProject.Core.DAL.EF;
using EducationProject.DAL.Mappings;
using EducationProject.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace Infrastructure.DAL.EF.Mappings
{
    /*
    public class SkillMappingEF : IMapping<SkillDBO>
    {
        private EducationProjectDbContext context;

        public SkillMappingEF(EducationProjectDbContext dbContext)
        {
            this.context = dbContext;
        }

        public bool Any(Expression<Func<SkillDBO, bool>> condition)
        {
            return this.context.Skills.Any(condition);
        }

        public void Create(SkillDBO entity)
        {
            this.context.Skills.Add(entity);
        }

        public void Delete(SkillDBO entity)
        {
            this.context.Skills.Remove(entity);
        }

        public void Delete(Expression<Func<SkillDBO, bool>> condition)
        {
            this.context.Skills
                .RemoveRange(this.context.Skills.Where(condition));
        }

        public void Delete(int id)
        {
            this.context.Skills
                .Remove(this.context.Skills.Find(id));
        }

        public void Delete(Predicate<SkillDBO> condition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SkillDBO> Get(Expression<Func<SkillDBO, bool>> condition, int pageNumber = 0, int pageSize = 30)
        {
            return this.context.Skills.Where(condition).Skip(pageNumber*pageSize).Take(pageSize);
        }

        public SkillDBO Get(int id)
        {
            return this.context.Skills.Find(id);
        }

        public IEnumerable<SkillDBO> Get(Predicate<SkillDBO> condition)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(SkillDBO entity)
        {
            this.context.Update(entity);
        }
    }
    */
}
