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
    public class CourseService : BaseService<CourseDBO, ShortCourseInfoDTO>, ICourseService
    {
        private IMaterialService materials;

        private ISkillService skills;

        private IRepository<CourseSkillDBO> courseSkills;

        private IRepository<CourseMaterialDBO> courseMaterials;

        public CourseService(IRepository<CourseDBO> baseEntityRepository, 
            AuthorizationService authorisztionService,
            IMaterialService materialService,
            ISkillService skillService,
            IRepository<CourseSkillDBO> courseSkillsRepository,
            IRepository<CourseMaterialDBO> courseMaterialsRepository) 
            : base(baseEntityRepository, authorisztionService)
        {
            materials = materialService;
            skills = skillService;

            this.courseSkills = courseSkillsRepository;

            this.courseMaterials = courseMaterialsRepository;
        }

        protected override Expression<Func<CourseDBO, ShortCourseInfoDTO>> FromBOMapping
        {
            get => c => new ShortCourseInfoDTO()
            {
                Id = c.Id,
                Description = c.Description,
                Title = c.Title
            };
        }

        protected override Expression<Func<CourseDBO, bool>> defaultGetCondition => c => c.IsVisible == true;

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

            courseMaterials.Create(new CourseMaterialDBO()
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

            courseMaterials.Delete(new CourseMaterialDBO()
            {
                CourseId = courseMaterialChange.CourseId,
                MaterialId = courseMaterialChange.MaterialId,
            });

            if(courseMaterials.Count(cm => cm.CourseId == courseMaterialChange.CourseId) == 1)
            {
                CourseDBO course = entity.Get(c => c.Id == courseMaterialChange.CourseId);
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

            courseSkills.Create(new CourseSkillDBO()
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

            courseSkills.Update(new CourseSkillDBO()
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

            if (courseMaterials.Count(cm => cm.CourseId == visibilityParams.CourseId) == 0)
            {
                return false;
            }

            CourseDBO course = entity.Get(c => c.Id == visibilityParams.CourseId);
            course.IsVisible = visibilityParams.Visibility;

            this.entity.Update(course);

            entity.Save();

            return true;
        }

        public FullCourseInfoDTO GetCourseInfo(int id)
        {
            return entity.Get<FullCourseInfoDTO>(c => c.Id == id,
                c => new FullCourseInfoDTO()
                {
                    Id = c.Id,
                    Description = c.Description,
                    Title = c.Title,
                    CreatorId = c.CreatorId,
                    Materials = c.CourseMaterials
                    .Where(cm => cm.CourseId == c.Id)
                    .Select(cm => new CourseMaterialDTO()
                    {
                        MaterialTitle = cm.Material.Title,
                        MaterialType = cm.Material.Type
                    })
                    .Take(defaultGetCount),
                    Skills = c.CourseSkills
                    .Where(cs => cs.CourseId == c.Id)
                    .Select(cs => new CourseSkillDTO()
                    {
                        SkillTitle = cs.Skill.Title,
                        SkillChange = cs.Change
                    })
                    .Take(defaultGetCount)
                });
        }

        public bool IsCourseContainsMaterial(ChangeCourseMaterialDTO courseMaterial)
        {
            if (courseMaterial.CourseId == 0 || courseMaterial.MaterialId == 0)
            {
                return false;
            }

            return this.courseMaterials.Contains(new CourseMaterialDBO()
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

        protected override CourseDBO Map(ShortCourseInfoDTO entity)
        {
            return new CourseDBO()
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

            if (this.IsExist(c => c.Id == courseMaterialChange.CourseId) == false
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

        protected override Expression<Func<CourseDBO, bool>> IsExistExpression(ShortCourseInfoDTO entity)
        {
            return c => c.Id == entity.Id;
        }
    }
}
