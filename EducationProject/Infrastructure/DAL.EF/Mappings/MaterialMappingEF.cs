using EducationProject.Core.DAL.EF;
using EducationProject.DAL.Mappings;
using EducationProject.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.DAL.EF.Mappings
{
    public class MaterialMappingEF : IMapping<BaseMaterialDBO>
    {
        private EducationProjectDbContext context;

        public MaterialMappingEF(EducationProjectDbContext dbContext)
        {
            context = dbContext;
        }

        public void Create(BaseMaterialDBO entity)
        {
            this.context.Materials.Add(entity);
        }

        public void Delete(BaseMaterialDBO entity)
        {
            this.context.Materials.Remove(entity);
        }

        public void Delete(Expression<Func<BaseMaterialDBO, bool>> condition)
        {
            this.context.Materials
                .RemoveRange(this.context.Materials.Where(condition));
        }

        public void Delete(int id)
        {
            this.context.Materials
                .Remove(this.context.Materials.Find(id));
        }

        public IEnumerable<BaseMaterialDBO> Get(Expression<Func<BaseMaterialDBO, bool>> condition)
        {
            return this.context.Materials.Where(condition);
        }

        public BaseMaterialDBO Get(int id)
        {
            return this.context.Materials.Find(id);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(BaseMaterialDBO entity)
        {
            this.context.Update(entity);
        }
    }
}
