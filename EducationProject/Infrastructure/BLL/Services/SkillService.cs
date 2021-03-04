using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using EducationProject.Core.Models;
using EducationProject.DAL.Interfaces;
using Infrastructure.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EducationProject.BLL;

namespace Infrastructure.BLL.Services
{
    public class SkillService : ISkillService
    {
        private IRepository<AccountSkill> accountSkillRepository;

        private IRepository<CourseSkill> courseSkillRepository;

        private IRepository<Skill> skillRepository;

        private IMapping<Skill, SkillDTO> skillMapping;

        public SkillService(IRepository<Skill> skillRepository,
            IRepository<AccountSkill> accountSkillRepository,
            IRepository<CourseSkill> courseSkillRepository,
            IMapping<Skill, SkillDTO> skillMapping)
        {
            this.accountSkillRepository = accountSkillRepository;

            this.courseSkillRepository = courseSkillRepository;

            this.skillRepository = skillRepository;

            this.skillMapping = skillMapping;
        }

        public async Task<IActionResult> CreateAsync(ChangeEntityDTO<SkillDTO> createEntity)
        {
            var isSkillExist = await this.skillRepository
                .AnyAsync(s => s.Title == createEntity.Entity.Title);

            if(isSkillExist)
            {
                return new ActionResult()
                {
                    IsSuccessful = false
                };
            }

            await this.skillRepository.CreateAsync(skillMapping.Map(createEntity.Entity));

            return new ActionResult()
            {
                IsSuccessful = true
            };
        }

        public async Task<IActionResult<IEnumerable<SkillDTO>>> GetAsync(PageInfoDTO pageInfo)
        {
            return new ActionResult<IEnumerable<SkillDTO>>()
            {
                IsSuccessful = true,
                Result = await this.skillRepository.GetPageAsync<SkillDTO>(s => true,
                   skillMapping.ConvertExpression, pageInfo.PageNumber, pageInfo.PageSize)
            };
        }

        public async Task<IActionResult> UpdateAsync(ChangeEntityDTO<SkillDTO> changeEntity)
        {
            await skillRepository.UpdateAsync(skillMapping.Map(changeEntity.Entity));

            return new ActionResult()
            {
                IsSuccessful = true
            };
        }

        public async Task<IActionResult> DeleteAsync(ChangeEntityDTO<SkillDTO> changeEntity)
        {
            await this.skillRepository.DeleteAsync(skillMapping.Map(changeEntity.Entity));

            return new ActionResult()
            {
                IsSuccessful = true
            };
        }

        public async Task<IActionResult<bool>> IsExistAsync(SkillDTO entity)
        {
            return new ActionResult<bool>()
            {
                IsSuccessful = true,
                Result = await this.skillRepository.AnyAsync(s => s.Id == entity.Id)
            };
        }

        public async Task<IActionResult> AddSkilsToAccountByCourseIdAsync(AddSkillsToAccountByCourseDTO changeSkills)
        {
            var skillsToAdd = await this.courseSkillRepository
                .GetPageAsync(cs => cs.CourseId == changeSkills.CourseId,
                cs => new { cs.Change, cs.SkillId }, 0, 
                await this.courseSkillRepository.CountAsync(cs => cs.CourseId == changeSkills.CourseId));

            skillsToAdd.ToList().ForEach(s =>
                AddSkillValueToAccountAsync(s.SkillId, changeSkills.AccountId, s.Change));

            return new ActionResult()
            {
                IsSuccessful = true
            };
        }

        public async Task<IActionResult<IEnumerable<AccountSkillDTO>>> GetAccountSkillsAsync(GetAccountSkillsDTO accountSkills)
        {
            return new ActionResult<IEnumerable<AccountSkillDTO>>()
            {
                IsSuccessful = true,
                Result = await this.accountSkillRepository.GetPageAsync<AccountSkillDTO>(a => a.AccountId == accountSkills.AccountId,
                a => new AccountSkillDTO()
                {
                    Title = a.Skill.Title,
                    SkillId = a.Skill.Id,
                    MaxValue = a.Skill.MaxValue,
                    CurrentResult = a.CurrentResult % a.Skill.MaxValue,
                    Level = a.CurrentResult / a.Skill.MaxValue
                }, accountSkills.PageNumber, accountSkills.PageSize)
            };
        }

        private async void AddSkillValueToAccountAsync(int skillId, int accountId, int value)
        {
            var isAccountSkillExists = await this.accountSkillRepository
                .AnyAsync(a => a.AccountId == accountId && a.SkillId == skillId);

            if (!isAccountSkillExists)
            {
                await this.accountSkillRepository.CreateAsync(new AccountSkill()
                {
                    AccountId = accountId,
                    SkillId = skillId,
                    CurrentResult = value
                });
            }
            else
            {
                var accountSkill = await this.accountSkillRepository.GetAsync(a => a.AccountId == accountId && a.SkillId == skillId);

                accountSkill.CurrentResult += value;

                await this.accountSkillRepository.UpdateAsync(accountSkill);
            }
        }
    }
}
