using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using EducationProject.Core.Models;
using EducationProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationProject.BLL;
using EducationProject.BLL.ActionResultMessages;
using EducationProject.Infrastructure.BLL.Mappings;
using System;

namespace EducationProject.Infrastructure.BLL.Services
{
    public class SkillService : BaseService, ISkillService
    {
        private IRepository<AccountSkill> accountSkillRepository;

        private IRepository<CourseSkill> courseSkillRepository;

        private IRepository<Skill> skillRepository;

        private IMapping<Skill, SkillDTO> skillMapping;

        private ServiceResultMessageCollection serviceResultMessages;

        public SkillService(
            IRepository<Skill> skillRepository,
            IRepository<AccountSkill> accountSkillRepository,
            IRepository<CourseSkill> courseSkillRepository,
            SkillMapping skillMapping,
            ServiceResultMessageCollection serviceResultMessageCollection)
        {
            this.accountSkillRepository = accountSkillRepository;

            this.courseSkillRepository = courseSkillRepository;

            this.skillRepository = skillRepository;

            this.skillMapping = skillMapping;

            this.serviceResultMessages = serviceResultMessageCollection;
        }

        public async Task<IServiceResult> CreateSkillAsync(SkillDTO skill)
        {
            try
            {
                var isSkillExist = await this.skillRepository
                .AnyAsync(s => s.Title == skill.Title);

                if (isSkillExist)
                {
                    return this.GetDefaultActionResult(
                        false,
                        this.serviceResultMessages.SkillTitleExist);
                }

                await this.skillRepository.CreateAsync(this.skillMapping.Map(skill));

                return ServiceResult.GetDefault(true);
            }
            catch(Exception ex)
            {
                return ServiceResult.GetDefault(false, ex.Message);
            }
        }

        public async Task<IServiceResult<SkillDTO>> GetSkillAsync(int skillId)
        {
            try
            {
                var skill = await this.skillRepository.GetAsync<SkillDTO>(
                    s => s.Id == skillId,
                    this.skillMapping.ConvertExpression);

                return new ServiceResult<SkillDTO>()
                {
                    IsSuccessful = true,
                    Result = skill
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<SkillDTO>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult> UpdateSkillAsync(SkillDTO skill)
        {
            try
            {
                var isSkillExist = await this.skillRepository
                .AnyAsync(s => s.Id == skill.Id);

                if (!isSkillExist)
                {
                    return ServiceResult.GetDefault(
                        false,
                        serviceResultMessages.SkillNotExist);
                }

                await this.skillRepository.UpdateAsync(this.skillMapping.Map(skill));

                return this.GetDefaultActionResult(true);
            }
            catch(Exception ex)
            {
                return ServiceResult.GetDefault(false, ex.Message);
            }
        }

        public async Task<IServiceResult> DeleteSkillAsync(int id)
        {
            try
            {
                await this.skillRepository.DeleteAsync(s => s.Id == id);

                return ServiceResult.GetDefault(true);
            }
            catch(Exception ex)
            {
                return ServiceResult.GetDefault(false, ex.Message);
            }
        }

        public async Task<IServiceResult<IEnumerable<SkillDTO>>> GetAsync(PageInfoDTO pageInfo)
        {
            return new ServiceResult<IEnumerable<SkillDTO>>()
            {
                IsSuccessful = true,
                Result = await this.skillRepository.GetPageAsync<SkillDTO>(
                   s => true,
                   this.skillMapping.ConvertExpression, 
                   pageInfo.PageNumber, 
                   pageInfo.PageSize)
            };
        }

        public async Task<IServiceResult> UpdateAsync(ChangeEntityDTO<SkillDTO> changeEntity)
        {
            var isSkillExist = await this.skillRepository
                .AnyAsync(s => s.Id == changeEntity.Entity.Id);

            if (!isSkillExist)
            {
                return this.GetDefaultActionResult(false);
            }

            await this.skillRepository.UpdateAsync(this.skillMapping.Map(changeEntity.Entity));

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult> DeleteAsync(ChangeEntityDTO<SkillDTO> changeEntity)
        {
            await this.skillRepository.DeleteAsync(this.skillMapping.Map(changeEntity.Entity));

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult<bool>> IsExistAsync(SkillDTO entity)
        {
            return new ServiceResult<bool>()
            {
                IsSuccessful = true,
                Result = await this.skillRepository.AnyAsync(s => s.Id == entity.Id)
            };
        }

        public async Task<IServiceResult> AddSkilsToAccountByCourseIdAsync(AddSkillsToAccountByCourseDTO changeSkills)
        {
            var courseSkillsCount = await this.courseSkillRepository.CountAsync(cs => cs.CourseId == changeSkills.CourseId);

            if(courseSkillsCount == 0)
            {
                return this.GetDefaultActionResult(true);
            }

            var skillsToAdd = await this.courseSkillRepository.GetPageAsync(
                cs => cs.CourseId == changeSkills.CourseId,
                cs => new { cs.Change, cs.SkillId }, 
                0,
                courseSkillsCount);

            skillsToAdd.ToList().ForEach(s =>
                this.AddSkillValueToAccountAsync(s.SkillId, changeSkills.AccountId, s.Change));

            await this.accountSkillRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult<IEnumerable<AccountSkillDTO>>> GetAccountSkillsAsync(GetAccountSkillsDTO accountSkills)
        {
            return new ServiceResult<IEnumerable<AccountSkillDTO>>()
            {
                IsSuccessful = true,
                Result = await this.accountSkillRepository.GetPageAsync<AccountSkillDTO>(
                    a => a.AccountId == accountSkills.AccountId,
                    a => new AccountSkillDTO()
                    {
                        Title = a.Skill.Title,
                        SkillId = a.Skill.Id,
                        MaxValue = a.Skill.MaxValue,
                        CurrentResult = a.CurrentResult % a.Skill.MaxValue,
                        Level = a.CurrentResult / a.Skill.MaxValue
                    }, 
                    accountSkills.PageNumber, 
                    accountSkills.PageSize)
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
