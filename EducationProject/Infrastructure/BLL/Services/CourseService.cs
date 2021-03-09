using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using EducationProject.Core.Models;
using EducationProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EducationProject.BLL;
using EducationProject.BLL.ActionResultMessages;
using EducationProject.Infrastructure.BLL.Mappings;
using System.Linq.Expressions;
using System.Linq;

namespace EducationProject.Infrastructure.BLL.Services
{
    public class CourseService : BaseService, ICourseService
    {
        private IMaterialService materialService;

        private ISkillService skillService;

        private IAccountService accountService;

        private IRepository<CourseSkill> courseSkillRepository;

        private IRepository<CourseMaterial> courseMaterialRepository;

        private IMapping<Course, ShortCourseInfoDTO> courseMapping;

        private IRepository<Course> courseRepository;

        private ServiceResultMessageCollection serviceResultMessages;

        private IAuthorizationService authorizationService;

        public CourseService(
            IRepository<Course> courseRepository,
            IMaterialService materialService,
            ISkillService skillService,
            IAccountService accountService,
            IRepository<CourseSkill> courseSkillRepository,
            IAuthorizationService authorizationService,
            IRepository<CourseMaterial> courseMaterialRepository,
            CourseMapping courseMapping,
            ServiceResultMessageCollection serviceResultMessageCollection)
        {
            this.materialService = materialService;

            this.courseRepository = courseRepository;

            this.authorizationService = authorizationService;

            this.materialService = materialService;

            this.skillService = skillService;

            this.courseSkillRepository = courseSkillRepository;

            this.courseMaterialRepository = courseMaterialRepository;

            this.accountService = accountService;

            this.courseMapping = courseMapping;

            this.serviceResultMessages = serviceResultMessageCollection;
        }
        
        public async Task<IServiceResult<EntityInfoPageDTO<ShortCourseInfoDTO>>> GetCoursePageAsync(PageInfoDTO pageInfo)
        {
            try
            {
                var pageCount = await this.GetPagesCountAsync(pageInfo.PageSize, t => true);

                if (pageInfo.PageNumber >= pageCount || pageInfo.PageNumber < 0)
                {
                    pageInfo.PageNumber = 0;
                }

                var courseInfoPage = new EntityInfoPageDTO<ShortCourseInfoDTO>()
                {
                    CurrentPage = pageInfo.PageNumber
                };

                courseInfoPage.CanMoveBack = pageInfo.PageNumber > 0;
                courseInfoPage.CanMoveForward = pageCount > pageInfo.PageNumber + 1;

                courseInfoPage.Entities = await this.courseRepository.GetPageAsync<ShortCourseInfoDTO>(
                    s => s.IsVisible == true,
                    this.courseMapping.ConvertExpression,
                    pageInfo.PageNumber,
                    pageInfo.PageSize);

                return new ServiceResult<EntityInfoPageDTO<ShortCourseInfoDTO>>()
                {
                    IsSuccessful = true,
                    Result = courseInfoPage
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<EntityInfoPageDTO<ShortCourseInfoDTO>>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult<FullCourseInfoDTO>> GetFullCourseInfoAsync(
            int courseId,
            PageInfoDTO materialPageInfo,
            PageInfoDTO skillPageInfo)
        {
            try
            {
                FullCourseInfoDTO result = await this.courseRepository.GetAsync<FullCourseInfoDTO>(
                c => c.Id == courseId,
                c => new FullCourseInfoDTO()
                {
                    Id = c.Id,
                    Description = c.Description,
                    Title = c.Title,
                    CreatorId = c.CreatorId
                });

                var skillServiceResult = await this.skillService.GetCourseSkillPageAsync(
                    courseId,
                    skillPageInfo);

                if (!skillServiceResult.IsSuccessful)
                {
                    return new ServiceResult<FullCourseInfoDTO>()
                    {
                        IsSuccessful = false,
                        MessageCode = skillServiceResult.MessageCode
                    };
                }

                result.CanBeChanged = this.authorizationService.GetAccountId() == result.CreatorId;

                result.Skills = skillServiceResult.Result;

                var materialServiceResult = await this.materialService.GetCourseMaterialPageAsync(
                    courseId,
                    this.authorizationService.GetAccountId(),
                    materialPageInfo);

                result.CanBePublished = materialServiceResult.Result.Entities.Any();

                if (!materialServiceResult.IsSuccessful)
                {
                    return new ServiceResult<FullCourseInfoDTO>()
                    {
                        IsSuccessful = false,
                        MessageCode = materialServiceResult.MessageCode
                    };
                }

                result.Materials = materialServiceResult.Result;

                result.CanBePassed = await this.materialService.IsAccountPassedAllCourseMaterialsAsync(
                    this.authorizationService.GetAccountId(),
                    courseId);

                return new ServiceResult<FullCourseInfoDTO>()
                {
                    IsSuccessful = true,
                    Result = result
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<FullCourseInfoDTO>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult<int>> CreateCourseAsync(ShortCourseInfoDTO course)
        {
            try
            {
                var isCourseTitleExist = await this.courseRepository.AnyAsync(c => c.Title == course.Title);

                if (isCourseTitleExist)
                {
                    return new ServiceResult<int>()
                    {
                        IsSuccessful = false,
                        MessageCode = this.serviceResultMessages.CourseTitleExist
                    };
                }

                var creatorId = this.authorizationService.GetAccountId();

                var courseToCreate = this.courseMapping.Map(course);

                courseToCreate.CreatorId = creatorId;
                courseToCreate.IsVisible = false;

                await this.courseRepository.CreateAsync(courseToCreate);

                await this.courseRepository.SaveAsync();

                return new ServiceResult<int>()
                {
                    IsSuccessful = true,
                    Result = courseToCreate.Id
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

        public async Task<IServiceResult> UpdateCourseAsync(ShortCourseInfoDTO course)
        {
            try
            {
                var isCourseExist = await this.courseRepository.AnyAsync(c => c.Id == course.Id);

                if(!isCourseExist)
                {
                    return ServiceResult.GetDefault(
                        false,
                        this.serviceResultMessages.CourseNotExist);
                }

                var isCourseTitleExist = await this.courseRepository.AnyAsync(
                    c => c.Title == course.Title && c.Id != course.Id);

                if (isCourseTitleExist)
                {
                    return ServiceResult.GetDefault(
                        false,
                        this.serviceResultMessages.CourseTitleExist);
                }

                var accountId = this.authorizationService.GetAccountId();

                var canAccountChangeCourse = await this.courseRepository.AnyAsync(
                    c => c.CreatorId == accountId && c.Id == course.Id);

                if(!canAccountChangeCourse)
                {
                    return ServiceResult.GetDefault(
                        false,
                        this.serviceResultMessages.PermissionDenied);
                }

                var courseToUpdate = await this.courseRepository.GetAsync(c => c.Id == course.Id);

                courseToUpdate.Title = course.Title;
                courseToUpdate.Description = course.Description;

                await this.courseRepository.UpdateAsync(courseToUpdate);

                await this.courseRepository.SaveAsync();

                return ServiceResult.GetDefault(true);
            }
            catch(Exception ex)
            {
                return ServiceResult.GetDefault(
                    false,
                    ex.Message);
            }
        }

        public async Task<IServiceResult> DeleteCourseAsync(int courseId)
        {
            try
            {
                var isCourseExist = await this.courseRepository.AnyAsync(c => c.Id == courseId);

                if (!isCourseExist)
                {
                    return ServiceResult.GetDefault(
                        false,
                        this.serviceResultMessages.CourseNotExist);
                }

                var accountId = this.authorizationService.GetAccountId();

                var canAccountChangeCourse = await this.courseRepository.AnyAsync(
                    c => c.CreatorId == accountId && c.Id == courseId);

                if (!canAccountChangeCourse)
                {
                    return ServiceResult.GetDefault(
                        false,
                        this.serviceResultMessages.PermissionDenied);
                }

                await this.courseRepository.DeleteAsync(c => c.Id == courseId);

                await this.courseRepository.SaveAsync();

                return ServiceResult.GetDefault(true);
            }
            catch(Exception ex)
            {
                return ServiceResult.GetDefault(
                    false,
                    ex.Message);
            }
        }

        public async Task<IServiceResult<ShortCourseInfoDTO>> GetCourseInfoAsync(int courseId)
        {
            try
            {
                return new ServiceResult<ShortCourseInfoDTO>()
                {
                    IsSuccessful = true,
                    Result = await this.courseRepository.GetAsync<ShortCourseInfoDTO>(
                        c => c.Id == courseId,
                        this.courseMapping.ConvertExpression)
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<ShortCourseInfoDTO>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult<IEnumerable<ShortCourseInfoDTO>>> GetCoursesByCreatorIdAsync(GetCoursesByCreatorDTO courseCreator)
        {
            return new ServiceResult<IEnumerable<ShortCourseInfoDTO>>()
            {
                IsSuccessful = true,
                Result = await this.courseRepository.GetPageAsync<ShortCourseInfoDTO>(
                    c => c.CreatorId == courseCreator.AccountId,
                    this.courseMapping.ConvertExpression, 
                    courseCreator.PageInfo.PageNumber, 
                    courseCreator.PageInfo.PageSize)
            };
        }

        public async Task<IServiceResult> AddCourseMaterialAsync(ChangeCourseMaterialDTO courseMaterialChange)
        {
            var isCourseAndMaterialExist = await this.CheckCourseMaterialsAsync(courseMaterialChange);

            if (!isCourseAndMaterialExist)
            {
                return this.GetDefaultActionResult(false);
            }

            var isAccountCanChangeCourse = await this.CheckCourseCreatorAsync(
                courseMaterialChange.CourseId,
                courseMaterialChange.AccountId);

            if (!isAccountCanChangeCourse)
            {
                return this.GetDefaultActionResult(false);
            }

            var isCourseMaterialAlreadyExist = await this.courseMaterialRepository.AnyAsync(c =>
                c.MaterialId == courseMaterialChange.MaterialId
                && c.CourseId == courseMaterialChange.CourseId);

            if (isCourseMaterialAlreadyExist)
            {
                return this.GetDefaultActionResult(false);
            }

            await this.courseMaterialRepository.CreateAsync(new CourseMaterial()
            {
                CourseId = courseMaterialChange.CourseId,
                MaterialId = courseMaterialChange.MaterialId
            });

            await this.courseMaterialRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }
        
        public async Task<IServiceResult> RemoveCourseMaterialAsync(ChangeCourseMaterialDTO courseMaterialChange)
        {
            var isAccountCanChangeCourse = await this.CheckCourseCreatorAsync(
                courseMaterialChange.CourseId,
               courseMaterialChange.AccountId);

            if (!isAccountCanChangeCourse)
            {
                return this.GetDefaultActionResult(false);
            }

            await this.courseMaterialRepository.DeleteAsync(new CourseMaterial()
            {
                CourseId = courseMaterialChange.CourseId,
                MaterialId = courseMaterialChange.MaterialId
            });

            await this.courseSkillRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult> AddCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange)
        {
            var isCourseAndSkillExist = await this.CheckCourseSkillsAsync(courseSkillChange);

            if (!isCourseAndSkillExist)
            {
                return this.GetDefaultActionResult(false);
            }

            var isAccountCanChangeCourse = await this.CheckCourseCreatorAsync(
                courseSkillChange.CourseId,
                courseSkillChange.AccountId);

            if (!isAccountCanChangeCourse)
            {
                return this.GetDefaultActionResult(false);
            }

            var isCourseSkillAlreadyExist = await this.courseSkillRepository.AnyAsync(c =>
                c.SkillId == courseSkillChange.SkillId 
                && c.CourseId == courseSkillChange.CourseId);

            if (isCourseSkillAlreadyExist)
            {
                return this.GetDefaultActionResult(false);
            }

            await this.courseSkillRepository.CreateAsync(new CourseSkill()
            {
                CourseId = courseSkillChange.CourseId,
                SkillId = courseSkillChange.SkillId,
                Change = courseSkillChange.Change
            });

            await this.courseSkillRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult> RemoveCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange)
        {
            var isAccountCanChangeCourse = await this.CheckCourseCreatorAsync(
                courseSkillChange.CourseId,
                courseSkillChange.AccountId);

            if (!isAccountCanChangeCourse)
            {
                return this.GetDefaultActionResult(false);
            }

            await this.courseSkillRepository.DeleteAsync(new CourseSkill()
            {
                CourseId = courseSkillChange.CourseId,
                SkillId = courseSkillChange.SkillId
            });

            await this.courseSkillRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult> ChangeCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange)
        {
            var isCourseAndSkillExist = await this.CheckCourseSkillsAsync(courseSkillChange);

            if (!isCourseAndSkillExist)
            {
                return this.GetDefaultActionResult(false);
            }

            var isAccountCanChangeCourse = await this.CheckCourseCreatorAsync(
                courseSkillChange.CourseId,
                courseSkillChange.AccountId);
            
            if (!isAccountCanChangeCourse)
            {
                return this.GetDefaultActionResult(false);
            }

            var isCourseSkillAlreadyExist = await this.courseSkillRepository.AnyAsync(cs =>
                cs.CourseId == courseSkillChange.CourseId && cs.SkillId == courseSkillChange.SkillId);

            if (!isCourseSkillAlreadyExist)
            {
                return this.GetDefaultActionResult(false);
            }

            await this.courseSkillRepository.UpdateAsync(new CourseSkill()
            {
                CourseId = courseSkillChange.CourseId,
                SkillId = courseSkillChange.SkillId,
                Change = courseSkillChange.Change
            });

            await this.courseSkillRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult> ChangeCourseVisibilityAsync(CourseVisibilityDTO visibilityParams)
        {
            var isCourseExist = await this.CheckCourseCreatorAsync(
                visibilityParams.CourseId, 
                visibilityParams.AccountId);

            if (!isCourseExist)
            {
                return this.GetDefaultActionResult(false);
            }

            var courseMaterialsCount = await this.courseMaterialRepository.CountAsync(cm =>
                cm.CourseId == visibilityParams.CourseId);

            if (courseMaterialsCount == 0)
            {
                return this.GetDefaultActionResult(false);
            }

            var courseToUpdate = await this.courseRepository.GetAsync(c => c.Id == visibilityParams.CourseId);

            courseToUpdate.IsVisible = visibilityParams.Visibility;

            await this.courseRepository.UpdateAsync(courseToUpdate);

            await this.courseRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult<bool>> IsCourseContainsMaterialAsync(ChangeCourseMaterialDTO courseMaterial)
        {
            return new ServiceResult<bool>()
            {
                IsSuccessful = true,
                Result = await this.courseMaterialRepository.ContainsAsync(new CourseMaterial()
                {
                    CourseId = courseMaterial.CourseId,
                    MaterialId = courseMaterial.MaterialId
                })
            }; 
        }

        public async Task<IServiceResult<bool>> IsCourseContainsMaterialAsync(IEnumerable<ChangeCourseMaterialDTO> courseMaterials)
        {
            foreach (var entity in courseMaterials)
            {
                var isCourseContainsMaterial = await this.IsCourseContainsMaterialAsync(entity);

                if (!isCourseContainsMaterial.Result)
                {
                    return new ServiceResult<bool>()
                    {
                        IsSuccessful = true,
                        Result = false
                    };
                }
            }

            return new ServiceResult<bool>()
            {
                IsSuccessful = true,
                Result = true
            };
        }

        public async Task<IServiceResult<bool>> IsExistAsync(ShortCourseInfoDTO checkEntity)
        {
            return new ServiceResult<bool>()
            {
                IsSuccessful = true,
                Result = await this.courseRepository.AnyAsync(c => c.Id == checkEntity.Id)
            };
        }
        private async Task<bool> CheckCourseCreatorAsync(int courseId, int accountId)
        {
            return await this.courseRepository.AnyAsync(c => c.Id == courseId && c.CreatorId == accountId);
        }

        private async Task<bool> CheckCourseMaterialsAsync(ChangeCourseMaterialDTO courseMaterialChange)
        {
            throw new NotImplementedException();
            /*
            var isCourseExist = await this.courseRepository.AnyAsync(c =>
                c.Id == courseMaterialChange.CourseId);

            var isMaterialExist = await this.materialService.IsExistAsync(new MaterialDTO()
            {
                Id = courseMaterialChange.MaterialId
            });

            if (!isCourseExist || !isMaterialExist.Result)
            {
                return false;
            }

            return true;*/
        }

        private async Task<bool> CheckCourseSkillsAsync(ChangeCourseSkillDTO courseSkillChange)
        {
            throw new NotImplementedException();
            /*
            var isCourseExist = await this.courseRepository.AnyAsync(c =>
                c.Id == courseSkillChange.CourseId);

            var isSkillExist = await this.skillService.IsExistAsync(new SkillDTO()
            {
                Id = courseSkillChange.SkillId
            });

            if (!isCourseExist || !isSkillExist.Result)
            {
                return false;
            }

            return true;*/
        }

        private async Task<int> GetPagesCountAsync(int pageSize, Expression<Func<Course, bool>> courseCondition)
        {
            var result = await this.courseRepository.CountAsync(courseCondition);

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
