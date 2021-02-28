using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using EducationProject.Core.DAL.EF;
using EducationProject.DAL.Interfaces;
using Infrastructure.DAL.EF.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.BLL.Services
{
    public class SkillService : BaseService<SkillDBO, SkillDTO>, ISkillService
    {
        public SkillService(IRepository<SkillDBO> baseEntityRepository,
            AuthorizationService authorisztionService)
            : base(baseEntityRepository, authorisztionService)
        {

        }

        protected override Expression<Func<SkillDBO, SkillDTO>> FromBOMapping
        {
            get => s => new SkillDTO()
            {
                Id = s.Id,
                Description = s.Description,
                MaxValue = s.MaxValue,
                Title = s.Title
            };
        }

        protected override Expression<Func<SkillDBO, bool>> IsExistExpression(SkillDTO entity)
        {
            return s => s.Id == entity.Id;
        }

        protected override SkillDBO Map(SkillDTO entity)
        {
            return new SkillDBO()
            {
                Id = entity.Id,
                Description = entity.Description,
                MaxValue = entity.MaxValue,
                Title = entity.Title
            };
        }

        protected override bool ValidateEntity(SkillDTO entity)
        {
            if(String.IsNullOrEmpty(entity.Title) == true || entity.MaxValue <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
