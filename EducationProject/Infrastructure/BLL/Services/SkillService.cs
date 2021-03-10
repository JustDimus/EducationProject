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
using System.Linq.Expressions;

namespace EducationProject.Infrastructure.BLL.Services
{
    public class SkillService : BaseService, ISkillService
    {
        private IRepository<AccountSkill> accountSkillRepository;

        private IRepository<CourseSkill> courseSkillRepository;

        private IRepository<Skill> skillRepository;

        private IAuthorizationService authorizationService;

        private SkillMapping skillMapping;

        private ServiceResultMessageCollection serviceResultMessages;

        public SkillService(
            IRepository<Skill> skillRepository,
            IRepository<AccountSkill> accountSkillRepository,
            IRepository<CourseSkill> courseSkillRepository,
            IAuthorizationService authorizationService,
            SkillMapping skillMapping,
            ServiceResultMessageCollection serviceResultMessageCollection)
        {
            this.accountSkillRepository = accountSkillRepository;

            this.courseSkillRepository = courseSkillRepository;

            this.skillRepository = skillRepository;

            this.authorizationService = authorizationService;

            this.skillMapping = skillMapping;

            this.serviceResultMessages = serviceResultMessageCollection;
        }

        public async Task<IServiceResult<EntityInfoPageDTO<CourseSkillDTO>>> GetCourseSkillPageAsync(int courseId, PageInfoDTO pageInfo)
        {
            try
            {
                var pageCount = await this.GetCourseSkillPagesCountAsync(
                    pageInfo.PageSize, 
                    cs => cs.CourseId == courseId);

                if (pageInfo.PageNumber >= pageCount || pageInfo.PageNumber < 0)
                {
                    pageInfo.PageNumber = 0;
                }

                var skillInfoPage = new EntityInfoPageDTO<CourseSkillDTO>()
                {
                    CurrentPage = pageInfo.PageNumber,
                    CurrentPageSize = pageInfo.PageSize
                };

                skillInfoPage.CanMoveBack = pageInfo.PageNumber > 0;
                skillInfoPage.CanMoveForward = pageCount > pageInfo.PageNumber + 1;

                skillInfoPage.Entities = (await this.courseSkillRepository.GetPageAsync<CourseSkillDTO>(
                    cs => cs.CourseId == courseId,
                    this.skillMapping.CourseSkillConvertExpression,
                    pageInfo.PageNumber,
                    pageInfo.PageSize)).ToList();

                return new ServiceResult<EntityInfoPageDTO<CourseSkillDTO>>()
                {
                    IsSuccessful = true,
                    Result = skillInfoPage
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<EntityInfoPageDTO<CourseSkillDTO>>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult<EntityInfoPageDTO<SkillDTO>>> GetSkillPageAsync(PageInfoDTO pageInfo)
        {
            try
            {
                var pageCount = await this.GetCourseSkillPagesCountAsync(pageInfo.PageSize, t => true);

                if(pageInfo.PageNumber >= pageCount || pageInfo.PageNumber < 0)
                {
                    pageInfo.PageNumber = 0;
                }

                var skillInfoPage = new EntityInfoPageDTO<SkillDTO>()
                {
                    CurrentPage = pageInfo.PageNumber,
                    CurrentPageSize = pageInfo.PageSize
                };

                skillInfoPage.CanMoveBack = pageInfo.PageNumber > 0;
                skillInfoPage.CanMoveForward = pageCount > pageInfo.PageNumber + 1;

                skillInfoPage.Entities = await this.skillRepository.GetPageAsync<SkillDTO>(
                    s => true,
                    this.skillMapping.ConvertExpression,
                    pageInfo.PageNumber,
                    pageInfo.PageSize);

                return new ServiceResult<EntityInfoPageDTO<SkillDTO>>()
                {
                    IsSuccessful = true,
                    Result = skillInfoPage
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<EntityInfoPageDTO<SkillDTO>>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult<int>> CreateSkillAsync(SkillDTO skill)
        {
            try
            {
                var isSkillExist = await this.skillRepository
                .AnyAsync(s => s.Title == skill.Title);

                if (isSkillExist)
                {
                    return new ServiceResult<int>()
                    {
                        IsSuccessful = false,
                        MessageCode = this.serviceResultMessages.SkillTitleExist
                    };
                }

                var skillToCreate = this.skillMapping.Map(skill);

                await this.skillRepository.CreateAsync(skillToCreate);

                await this.skillRepository.SaveAsync();

                return new ServiceResult<int>()
                {
                    IsSuccessful = true,
                    Result = skillToCreate.Id
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<int>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };

            }
        }

        public async Task<IServiceResult<SkillDTO>> GetSkillAsync(int skillId)
        {
            try
            {
                var skill = await this.skillRepository.GetAsync<SkillDTO>(
                    s => s.Id == skillId,
                    this.skillMapping.ConvertExpression);

                if (skill == null)
                {
                    return new ServiceResult<SkillDTO>()
                    {
                        IsSuccessful = false,
                        MessageCode = this.serviceResultMessages.SkillNotExist
                    };
                }

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

                await this.skillRepository.SaveAsync();

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

        public async Task<IServiceResult> DeleteAsync(ChangeEntityDTO<SkillDTO> changeEntity)
        {
            await this.skillRepository.DeleteAsync(this.skillMapping.Map(changeEntity.Entity));

            return this.GetDefaultActionResult(true);
        }

        public async Task<bool> IsExistAsync(SkillDTO skill)
        {
            try
            {
                return await this.skillRepository.AnyAsync(s => s.Id == skill.Id);
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task<IServiceResult> AddSkilsToAccountByCourseIdAsync(AddSkillsToAccountByCourseDTO changeSkills)
        {
            var courseSkillsCount = await this.courseSkillRepository.CountAsync(cs => cs.CourseId == changeSkills.CourseId);

            if(courseSkillsCount == 0)
            {
                return this.GetDefaultActionResult(true);
            }

            var skillsToAdd = (await this.courseSkillRepository.GetPageAsync(
                cs => cs.CourseId == changeSkills.CourseId,
                cs => new { cs.Change, cs.SkillId }, 
                0,
                courseSkillsCount)).ToList();

            foreach(var skill in skillsToAdd)
            {
                await this.AddSkillValueToAccountAsync(
                    skill.SkillId,
                    changeSkills.AccountId,
                    skill.Change);
            }

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

        private async Task AddSkillValueToAccountAsync(int skillId, int accountId, int value)
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

        private async Task<int> GetPagesCountAsync(int pageSize, Expression<Func<Skill, bool>> skillCondition)
        {
            var result = await this.skillRepository.CountAsync(skillCondition);

            if (result % pageSize == 0)
            {
                result = result / pageSize;
            }
            else
            {
                result = (result / pageSize) + 1;
            }

            return result;
        }
   
        private async Task<int> GetCourseSkillPagesCountAsync(int pageSize, Expression<Func<CourseSkill, bool>> condition)
        {
            var result = await this.courseSkillRepository.CountAsync(condition);

            if (result % pageSize == 0)
            {
                result = result / pageSize;
            }
            else
            {
                result = (result / pageSize) + 1;
            }

            return result;
        }
    }
}
