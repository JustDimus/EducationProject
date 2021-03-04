﻿using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using EducationProject.Core.Models;
using EducationProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EducationProject.BLL;

namespace Infrastructure.BLL.Services
{
    public class CourseService : ICourseService
    {
        private IMaterialService materialService;

        private ISkillService skillService;

        private IRepository<CourseSkill> courseSkillRepository;

        private IRepository<CourseMaterial> courseMaterialRepository;

        private IMapping<Course, ShortCourseInfoDTO> courseMapping;

        private IRepository<Course> courseRepository;

        private int defaultPageSize;

        public CourseService(
            IRepository<Course> courseRepository,
            IMaterialService materialService,
            ISkillService skillService,
            IRepository<CourseSkill> courseSkillRepository,
            IRepository<CourseMaterial> courseMaterialRepository,
            IMapping<Course, ShortCourseInfoDTO> courseMapping,
            int defaultPageSize)
        {
            this.materialService = materialService;

            this.courseRepository = courseRepository;

            this.materialService = materialService;

            this.skillService = skillService;

            this.courseSkillRepository = courseSkillRepository;

            this.courseMaterialRepository = courseMaterialRepository;

            this.defaultPageSize = defaultPageSize;

            this.courseMapping = courseMapping;
        }
      
        public async Task<IActionResult<IEnumerable<ShortCourseInfoDTO>>> GetCoursesByCreatorIdAsync(GetCoursesByCreatorDTO courseCreator)
        {
            return new ActionResult<IEnumerable<ShortCourseInfoDTO>>()
            {
                IsSuccessful = true,
                Result = await this.courseRepository.GetPageAsync<ShortCourseInfoDTO>(
                    c => c.CreatorId == courseCreator.AccountId,
                    this.courseMapping.ConvertExpression, 
                    courseCreator.PageInfo.PageNumber, 
                    courseCreator.PageInfo.PageSize)
            };
        }

        public async Task<IActionResult> AddCourseMaterialAsync(ChangeCourseMaterialDTO courseMaterialChange)
        {
            var isCourseAndMaterialExist = await this.CheckCourseMaterialsAsync(courseMaterialChange);

            if (!isCourseAndMaterialExist)
            {
                return this.GetDefaultActionResult(false, "Such course or material doesn't exist");
            }

            var isAccountCanChangeCourse = await this.CheckCourseCreatorAsync(
                courseMaterialChange.CourseId,
                courseMaterialChange.AccountId);

            if (!isAccountCanChangeCourse)
            {
                return this.GetDefaultActionResult(false, "This account can't change this course");
            }

            var isCourseMaterialAlreadyExist = await this.courseMaterialRepository.AnyAsync(c =>
                c.MaterialId == courseMaterialChange.MaterialId
                && c.CourseId == courseMaterialChange.CourseId);

            if (isCourseMaterialAlreadyExist)
            {
                return this.GetDefaultActionResult(false, "This course already has this material");
            }

            await this.courseMaterialRepository.CreateAsync(new CourseMaterial()
            {
                CourseId = courseMaterialChange.CourseId,
                MaterialId = courseMaterialChange.MaterialId
            });

            await this.courseMaterialRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }
        
        public async Task<IActionResult> RemoveCourseMaterialAsync(ChangeCourseMaterialDTO courseMaterialChange)
        {
            var isAccountCanChangeCourse = await this.CheckCourseCreatorAsync(
                courseMaterialChange.CourseId,
               courseMaterialChange.AccountId);

            if (!isAccountCanChangeCourse)
            {
                return this.GetDefaultActionResult(false, "This account can't change this course");
            }

            await this.courseMaterialRepository.DeleteAsync(new CourseMaterial()
            {
                CourseId = courseMaterialChange.CourseId,
                MaterialId = courseMaterialChange.MaterialId
            });

            await this.courseSkillRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IActionResult> AddCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange)
        {
            var isCourseAndSkillExist = await this.CheckCourseSkillsAsync(courseSkillChange);

            if (!isCourseAndSkillExist)
            {
                return this.GetDefaultActionResult(false, "Such course or skill doesn't exist");
            }

            var isAccountCanChangeCourse = await this.CheckCourseCreatorAsync(
                courseSkillChange.CourseId,
                courseSkillChange.AccountId);

            if (!isAccountCanChangeCourse)
            {
                return this.GetDefaultActionResult(false, "This account can't change this course");
            }

            var isCourseSkillAlreadyExist = await this.courseSkillRepository.AnyAsync(c =>
                c.SkillId == courseSkillChange.SkillId 
                && c.CourseId == courseSkillChange.CourseId);

            if (isCourseSkillAlreadyExist)
            {
                return this.GetDefaultActionResult(false, "This course already has this skill");
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

        public async Task<IActionResult> RemoveCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange)
        {
            var isAccountCanChangeCourse = await this.CheckCourseCreatorAsync(
                courseSkillChange.CourseId,
                courseSkillChange.AccountId);

            if (!isAccountCanChangeCourse)
            {
                return this.GetDefaultActionResult(false, "This account can't change this course");
            }

            await this.courseSkillRepository.DeleteAsync(new CourseSkill()
            {
                CourseId = courseSkillChange.CourseId,
                SkillId = courseSkillChange.SkillId
            });

            await this.courseSkillRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IActionResult> ChangeCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange)
        {
            var isCourseAndSkillExist = await this.CheckCourseSkillsAsync(courseSkillChange);

            if (!isCourseAndSkillExist)
            {
                return this.GetDefaultActionResult(false, "Such course or skill doesn't exist");
            }

            var isAccountCanChangeCourse = await this.CheckCourseCreatorAsync(
                courseSkillChange.CourseId,
                courseSkillChange.AccountId);
            
            if (!isAccountCanChangeCourse)
            {
                return this.GetDefaultActionResult(false, "This account can't change this course");
            }

            var isCourseSkillAlreadyExist = await this.courseSkillRepository.AnyAsync(cs =>
                cs.CourseId == courseSkillChange.CourseId && cs.SkillId == courseSkillChange.SkillId);

            if (!isCourseSkillAlreadyExist)
            {
                return this.GetDefaultActionResult(false, "That course doesn't have this material");
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

        public async Task<IActionResult> ChangeCourseVisibilityAsync(CourseVisibilityDTO visibilityParams)
        {
            var isCourseExist = await this.CheckCourseCreatorAsync(
                visibilityParams.CourseId, 
                visibilityParams.AccountId);

            if (!isCourseExist)
            {
                return this.GetDefaultActionResult(false, "Such course doesn't exist");
            }

            var isAccountCanChangeCourse = await this.CheckCourseCreatorAsync(
                visibilityParams.CourseId,
                visibilityParams.AccountId);

            if (!isAccountCanChangeCourse)
            {
                return this.GetDefaultActionResult(false, "This account can't change this course");
            }

            var courseMaterialsCount = await this.courseMaterialRepository.CountAsync(cm =>
                cm.CourseId == visibilityParams.CourseId);

            if (courseMaterialsCount == 0)
            {
                return this.GetDefaultActionResult(false, "That course doesn't contain any material");
            }

            var courseToUpdate = await this.courseRepository.GetAsync(c => c.Id == visibilityParams.CourseId);

            courseToUpdate.IsVisible = visibilityParams.Visibility;

            await this.courseRepository.UpdateAsync(courseToUpdate);

            await this.courseRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IActionResult<FullCourseInfoDTO>> GetCourseInfoAsync(int id)
        {
            FullCourseInfoDTO result = await this.courseRepository.GetAsync<FullCourseInfoDTO>(
                c => c.Id == id,
                c => new FullCourseInfoDTO()
                {
                    Id = c.Id,
                    Description = c.Description,
                    Title = c.Title,
                    CreatorId = c.CreatorId
                });

            result.Materials = await this.courseMaterialRepository.GetPageAsync<CourseMaterialDTO>(
                cm => cm.CourseId == id,
                cm => new CourseMaterialDTO()
                {
                    MaterialTitle = cm.Material.Title,
                    MaterialId = cm.Material.Id,
                    MaterialType = cm.Material.Type
                }, 
                0,
                this.defaultPageSize);

            result.Skills = await this.courseSkillRepository.GetPageAsync<CourseSkillDTO>(
                cs => cs.CourseId == id,
                cs => new CourseSkillDTO()
                {
                    SkillId = cs.Skill.Id,
                    SkillChange = cs.Change,
                    SkillTitle = cs.Skill.Title
                }, 
                0,
                this.defaultPageSize);

            return new ActionResult<FullCourseInfoDTO>()
            {
                IsSuccessful = true,
                Result = result
            };
        }

        public async Task<IActionResult<bool>> IsCourseContainsMaterialAsync(ChangeCourseMaterialDTO courseMaterial)
        {
            return new ActionResult<bool>()
            {
                IsSuccessful = true,
                Result = await this.courseMaterialRepository.ContainsAsync(new CourseMaterial()
                {
                    CourseId = courseMaterial.CourseId,
                    MaterialId = courseMaterial.MaterialId
                })
            }; 
        }

        public async Task<IActionResult<bool>> IsCourseContainsMaterialAsync(IEnumerable<ChangeCourseMaterialDTO> courseMaterials)
        {
            foreach (var entity in courseMaterials)
            {
                var isCourseContainsMaterial = await this.IsCourseContainsMaterialAsync(entity);

                if (!isCourseContainsMaterial.Result)
                {
                    return new ActionResult<bool>()
                    {
                        IsSuccessful = true,
                        Result = false
                    };
                }
            }

            return new ActionResult<bool>()
            {
                IsSuccessful = true,
                Result = true
            };
        }

        public async Task<IActionResult> CreateAsync(ChangeEntityDTO<ShortCourseInfoDTO> createEntity)
        {
            var courseToCreate = this.courseMapping.Map(createEntity.Entity);

            courseToCreate.CreatorId = createEntity.AccountId;

            await this.courseRepository.CreateAsync(courseToCreate);

            await this.courseRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IActionResult> UpdateAsync(ChangeEntityDTO<ShortCourseInfoDTO> updateEntity)
        {
            var isCourseExist = await this.CheckCourseCreatorAsync(updateEntity.Entity.Id, updateEntity.AccountId);

            if (!isCourseExist)
            {
                return this.GetDefaultActionResult(false, "Such course doesn't exist");
            }

            await this.courseRepository.UpdateAsync(this.courseMapping.Map(updateEntity.Entity));

            return this.GetDefaultActionResult(true);
        }

        public async Task<IActionResult> DeleteAsync(ChangeEntityDTO<ShortCourseInfoDTO> deleteEntity)
        {
            var isCourseExist = await this.CheckCourseCreatorAsync(deleteEntity.Entity.Id, deleteEntity.AccountId);

            if (!isCourseExist)
            {
                return this.GetDefaultActionResult(false, "Such course doesn't exist");
            }

            await this.courseRepository.DeleteAsync(this.courseMapping.Map(deleteEntity.Entity));

            return this.GetDefaultActionResult(true);
        }

        public async Task<IActionResult<bool>> IsExistAsync(ShortCourseInfoDTO checkEntity)
        {
            return new ActionResult<bool>()
            {
                IsSuccessful = true,
                Result = await this.courseRepository.AnyAsync(c => c.Id == checkEntity.Id)
            };
        }

        public async Task<IActionResult<IEnumerable<ShortCourseInfoDTO>>> GetAsync(PageInfoDTO pageInfo)
        {
            return new ActionResult<IEnumerable<ShortCourseInfoDTO>>()
            {
                IsSuccessful = true,
                Result = await this.courseRepository.GetPageAsync<ShortCourseInfoDTO>(
                    c => c.IsVisible == true,
                    this.courseMapping.ConvertExpression, 
                    pageInfo.PageNumber, 
                    pageInfo.PageSize)
            };
        }

        private async Task<bool> CheckCourseCreatorAsync(int courseId, int accountId)
        {
            return await this.courseRepository.AnyAsync(c => c.Id == courseId && c.CreatorId == accountId);
        }

        private async Task<bool> CheckCourseMaterialsAsync(ChangeCourseMaterialDTO courseMaterialChange)
        {
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

            return true;
        }

        private async Task<bool> CheckCourseSkillsAsync(ChangeCourseSkillDTO courseSkillChange)
        {
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

            return true;
        }

        private IActionResult GetDefaultActionResult(bool actionStatus, string message = null)
        {
            return new ActionResult()
            {
                IsSuccessful = actionStatus,
                ResultMessage = message
            };
        }
    }
}
