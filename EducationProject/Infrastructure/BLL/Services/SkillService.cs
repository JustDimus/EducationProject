using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using EducationProject.Core.DAL.EF;
using EducationProject.DAL.Interfaces;
using Infrastructure.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.BLL.Services
{
    public class SkillService : BaseService<SkillDBO, SkillDTO>, ISkillService
    {
        private IRepository<AccountSkillDBO> accountSkills;

        private IRepository<CourseSkillDBO> courseSkills;

        public SkillService(IRepository<SkillDBO> baseEntityRepository,
            AuthorizationService authorisztionService,
            IRepository<AccountSkillDBO> accountSkillRepository,
            IRepository<CourseSkillDBO> courseSkillRepository)
            : base(baseEntityRepository, authorisztionService)
        {
            this.accountSkills = accountSkillRepository;

            this.courseSkills = courseSkillRepository;
        }

        public override bool Create(ChangeEntityDTO<SkillDTO> createEntity)
        {
            if(this.entity.Any(s => s.Title == createEntity.Entity.Title) == true)
            {
                return false;
            }

            return base.Create(createEntity);
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

        public bool AddSkilsToAccountByCourseId(AddSkillsToAccountByCourseDTO changeSkills)
        {
            if(changeSkills.AccountId <= 0 && changeSkills.CourseId <= 0)
            {
                return false;
            }

            var skillsToAdd = courseSkills.GetPage(cs => cs.CourseId == changeSkills.CourseId,
                cs => new { cs.Change, cs.SkillId }, 0, 
                courseSkills.Count(cs => cs.CourseId == changeSkills.CourseId)).ToList();

            foreach(var skill in skillsToAdd)
            {
                AddSkillValueToAccount(skill.SkillId, changeSkills.AccountId, skill.Change);
            }

            return true;
        }

        public IEnumerable<AccountSkillDTO> GetAccountSkills(GetAccountSkillsDTO accountSkills)
        {
            return this.accountSkills.GetPage<AccountSkillDTO>(a => a.AccountId == accountSkills.AccountId,
                a => new AccountSkillDTO()
                {
                    Title = a.Skill.Title,
                    SkillId = a.Skill.Id,
                    MaxValue = a.Skill.MaxValue,
                    CurrentResult = a.CurrentResult % a.Skill.MaxValue,
                    Level = a.CurrentResult / a.Skill.MaxValue
                }, accountSkills.PageNumber, accountSkills.PageSize);
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

        private void AddSkillValueToAccount(int skillId, int accountId, int value)
        {
            if(accountSkills.Any(a => a.AccountId == accountId && a.SkillId == skillId) == false)
            {
                accountSkills.Create(new AccountSkillDBO()
                {
                    AccountId = accountId,
                    SkillId = skillId,
                    CurrentResult = value
                });
            }
            else
            {
                var accountSkill = accountSkills.Get(a => a.AccountId == accountId && a.SkillId == skillId);

                accountSkill.CurrentResult += value;

                accountSkills.Update(accountSkill);
            }
        }
    }
}
