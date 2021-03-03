using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using EducationProject.Core.DAL;
using EducationProject.DAL.Interfaces;
using Infrastructure.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.BLL.Services
{
    public class CourseService : BaseService<Course, ShortCourseInfoDTO>, ICourseService
    {
        private IMaterialService materials;

        private ISkillService skills;

        private IRepository<CourseSkill> courseSkills;

        private IRepository<CourseMaterial> courseMaterials;

        private int defaultPageSize;

        public CourseService(IRepository<Course> baseEntityRepository, 
            AuthorizationService authorisztionService,
            IMaterialService materialService,
            ISkillService skillService,
            IRepository<CourseSkill> courseSkillsRepository,
            IRepository<CourseMaterial> courseMaterialsRepository,
            int defaultPageSize) 
            : base(baseEntityRepository, authorisztionService)
        {
            materials = materialService;
            skills = skillService;

            this.courseSkills = courseSkillsRepository;

            this.courseMaterials = courseMaterialsRepository;

            this.defaultPageSize = defaultPageSize;
        }

        protected override Expression<Func<Course, ShortCourseInfoDTO>> FromBOMapping
        {
            get => c => new ShortCourseInfoDTO()
            {
                Id = c.Id,
                Description = c.Description,
                Title = c.Title
            };
        }

        protected override Expression<Func<Course, bool>> defaultGetCondition => c => c.IsVisible == true;

        public IEnumerable<ShortCourseInfoDTO> GetCoursesByCreatorId(GetCoursesByCreator courseCreator)
        {
            if(ValidatePageInfo(courseCreator.PageInfo) == false)
            {
                return null;
            }

            return entity.GetPage<ShortCourseInfoDTO>(c => c.CreatorId == courseCreator.AccountId,
                FromBOMapping, courseCreator.PageInfo.PageNumber, courseCreator.PageInfo.PageSize);
        }

        public IEnumerable<ShortCourseInfoDTO> GetMyCourses(GetCoursesByCreator courseCreator)
        {
            int accountId = this.Authenticate(courseCreator.Token);

            if(accountId == 0)
            {
                return null;
            }
            else
            {
                courseCreator.AccountId = accountId;

                return GetCoursesByCreatorId(courseCreator);
            }
        }

        public override bool Create(ChangeEntityDTO<ShortCourseInfoDTO> createEntity)
        {
            int accountId = 0;

            if (this.createCheckAuth == true)
            {
                accountId = this.authService.AuthenticateAccount(createEntity.Token);
                if(accountId == 0)
                { 
                    return false;
                }
            }

            if (ValidateEntity(createEntity.Entity) == false)
            {
                return false;
            }

            var course = Map(createEntity.Entity);

            course.CreatorId = accountId;

            entity.Create(course);

            entity.Save();

            return true;
        }

        public bool AddCourseMaterial(ChangeCourseMaterialDTO courseMaterialChange)
        {
            int accountId = Authenticate(courseMaterialChange.Token);

            if (accountId == 0)
            {
                return false;
            }

            if (CheckCourseMaterials(courseMaterialChange, accountId) == false)
            {
                return false;
            }

            if(courseMaterials.Any(cm => cm.CourseId == courseMaterialChange.CourseId
            && cm.MaterialId == courseMaterialChange.MaterialId) == true)
            {
                return false;
            }

            courseMaterials.Create(new CourseMaterial()
            {
                CourseId = courseMaterialChange.CourseId,
                MaterialId = courseMaterialChange.MaterialId,
            });

            entity.Save();

            return true;
        }
        
        public bool RemoveCourseMaterial(ChangeCourseMaterialDTO courseMaterialChange)
        {
            int accountId = Authenticate(courseMaterialChange.Token);

            if (accountId == 0)
            {
                return false;
            }

            if (CheckCourseMaterials(courseMaterialChange, accountId) == false)
            {
                return false;
            }

            courseMaterials.Delete(new CourseMaterial()
            {
                CourseId = courseMaterialChange.CourseId,
                MaterialId = courseMaterialChange.MaterialId,
            });

            if(courseMaterials.Count(cm => cm.CourseId == courseMaterialChange.CourseId) == 1)
            {
                Course course = entity.Get(c => c.Id == courseMaterialChange.CourseId);
                course.IsVisible = false;

                this.entity.Update(course);
            }

            entity.Save();

            return true;
        }

        public bool AddCourseSkill(ChangeCourseSkillDTO courseSkillChange)
        {
            int accountId = Authenticate(courseSkillChange.Token);

            if (accountId == 0)
            {
                return false;
            }

            if (CheckCourseSkills(courseSkillChange, accountId) == false
                || courseSkillChange.Change <= 0)
            {
                return false;
            }

            if(courseSkills.Any(cs => cs.CourseId == courseSkillChange.CourseId
            && cs.SkillId == courseSkillChange.SkillId) == true)
            {
                return false;
            }

            courseSkills.Create(new CourseSkill()
            {
                CourseId = courseSkillChange.CourseId,
                SkillId = courseSkillChange.SkillId,
                Change = courseSkillChange.Change
            });

            courseSkills.Save();

            return true;
        }

        public bool RemoveCourseSkill(ChangeCourseSkillDTO courseSkillChange)
        {
            int accountId = Authenticate(courseSkillChange.Token);

            if (accountId == 0)
            {
                return false;
            }

            if (CheckCourseSkills(courseSkillChange, accountId) == false)
            {
                return false;
            }

            if(courseSkills.Any(cs => cs.CourseId == courseSkillChange.CourseId
            && cs.SkillId == courseSkillChange.SkillId) == false)
            {
                return false;
            }

            courseSkills.Delete(cs => cs.CourseId == courseSkillChange.CourseId
            && cs.SkillId == courseSkillChange.SkillId);

            courseSkills.Save();

            return true;
        }

        public bool ChangeCourseSkill(ChangeCourseSkillDTO courseSkillChange)
        {
            int accountId = Authenticate(courseSkillChange.Token);

            if (accountId == 0)
            {
                return false;
            }

            if (CheckCourseSkills(courseSkillChange, accountId) == false
                || courseSkillChange.Change <= 0)
            {
                return false;
            }

            if (courseSkills.Any(cs => cs.CourseId == courseSkillChange.CourseId
             && cs.SkillId == courseSkillChange.SkillId) == false)
            {
                return false;
            }

            courseSkills.Update(new CourseSkill()
            {
                CourseId = courseSkillChange.CourseId,
                SkillId = courseSkillChange.SkillId,
                Change = courseSkillChange.Change
            });

            courseSkills.Save();

            return true;
        }

        public bool ChangeCourseVisibility(CourseVisibilityDTO visibilityParams)
        {
            int accountId = Authenticate(visibilityParams.Token);

            if (accountId == 0)
            {
                return false;
            }

            if(this.entity.Any(c => c.Id == visibilityParams.CourseId && c.CreatorId == accountId) == false)
            {
                return false;
            }

            if (courseMaterials.Count(cm => cm.CourseId == visibilityParams.CourseId) == 0)
            {
                return false;
            }

            Course course = entity.Get(c => c.Id == visibilityParams.CourseId);
            course.IsVisible = visibilityParams.Visibility;

            this.entity.Update(course);

            entity.Save();

            return true;
        }

        public FullCourseInfoDTO GetCourseInfo(int id)
        {
            if(entity.Any(c => c.Id == id) == false)
            {
                return null;
            }

            var result = entity.Get<FullCourseInfoDTO>(c => c.Id == id,
                c => new FullCourseInfoDTO()
                {
                    Id = c.Id,
                    Description = c.Description,
                    Title = c.Title,
                    CreatorId = c.CreatorId
                });

            result.Materials = courseMaterials.GetPage<CourseMaterialDTO>(cm => cm.CourseId == id,
                cm => new CourseMaterialDTO()
                {
                    MaterialTitle = cm.Material.Title,
                    MaterialId = cm.Material.Id,
                    MaterialType = cm.Material.Type
                }, 0, defaultPageSize);

            result.Skills = courseSkills.GetPage<CourseSkillDTO>(cs => cs.CourseId == id,
                cs => new CourseSkillDTO()
                {
                    SkillId = cs.Skill.Id,
                    SkillChange = cs.Change,
                    SkillTitle = cs.Skill.Title
                }, 0, defaultPageSize);

            return result;
        }

        public bool IsCourseContainsMaterial(ChangeCourseMaterialDTO courseMaterial)
        {
            if (courseMaterial.CourseId == 0 || courseMaterial.MaterialId == 0)
            {
                return false;
            }

            return this.courseMaterials.Contains(new CourseMaterial()
            {
                CourseId = courseMaterial.CourseId,
                MaterialId = courseMaterial.MaterialId
            });
        }

        public bool IsCourseContainsMaterial(IEnumerable<ChangeCourseMaterialDTO> courseMaterials)
        {
            foreach (var entity in courseMaterials)
            {
                if (IsCourseContainsMaterial(entity) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<int> GetAllCourseMaterialsId(int courseId)
        {
            return this.courseMaterials.GetPage<int>(cm => cm.CourseId == courseId, cm => cm.MaterialId, 0,
                this.courseMaterials.Count(cm => cm.CourseId == courseId));
        }

        protected override Course Map(ShortCourseInfoDTO entity)
        {
            return new Course()
            {
                Id = entity.Id,
                Description = entity.Description,
                Title = entity.Title,
                CreatorId = entity.CreatorId,
                IsVisible = false
            };
        }

        protected override bool ValidateEntity(ShortCourseInfoDTO entity)
        {
            if(String.IsNullOrEmpty(entity.Title) == true || String.IsNullOrEmpty(entity.Description) == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckCourseMaterials(ChangeCourseMaterialDTO courseMaterialChange, int accountId)
        {
            if (courseMaterialChange.CourseId <= 0 
                || courseMaterialChange.MaterialId <= 0)
            {
                return false;
            }

            if (this.IsExist(c => c.Id == courseMaterialChange.CourseId && c.CreatorId == accountId) == false
                || materials.IsExist(new MaterialDTO() { Id = courseMaterialChange.MaterialId }) == false)
            {
                return false;
            }

            return true;
        }

        private bool CheckCourseSkills(ChangeCourseSkillDTO courseSkillChange, int accountId)
        {
            if (courseSkillChange.CourseId <= 0 
                || courseSkillChange.SkillId <= 0)
            {
                return false;
            }

            if (this.IsExist(c => c.Id == courseSkillChange.CourseId
            && c.CreatorId == accountId) == false)
            {
                return false;
            }

            if(skills.IsExist(new SkillDTO() { Id = courseSkillChange.SkillId }) == false)
            {
                return false;
            }

            return true;
        }

        protected override Expression<Func<Course, bool>> IsExistExpression(ShortCourseInfoDTO entity)
        {
            return c => c.Id == entity.Id;
        }
    }
}
