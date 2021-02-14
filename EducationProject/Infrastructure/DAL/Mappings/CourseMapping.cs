using EducationProject.Core.BLL;
using EducationProject.Core.DAL;
using EducationProject.DAL.Mappings;
using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.DAL.Mappings
{
    public class CourseMapping : IMapping<CourseBO>
    {
        private UnitOfWork _uow;

        private IMapping<BaseMaterial> _materials;

        private IMapping<SkillBO> _skills;

        public CourseMapping(UnitOfWork UOW, IMapping<BaseMaterial> materials, IMapping<SkillBO> skills)
        {
            _uow = UOW;

            _materials = materials;

            _skills = skills;
        }

        public void Create(CourseBO Entity)
        {
            var course = new EducationProject.Core.DAL.CourseDBO()
            {
                Description = Entity.Description,
                CreatorId = Entity.CreatorId,
                IsVisible = Entity.IsVisible,
                Title = Entity.Title
            };

            _uow.Repository<EducationProject.Core.DAL.CourseDBO>().Create(course);

            Entity.Id = course.Id;

            if (Entity.Materials != null)
            {
                foreach (var material in Entity.Materials)
                {
                    _uow.Repository<CourseMaterialDBO>()
                        .Create(new CourseMaterialDBO()
                        {
                            CourseId = Entity.Id,
                            MaterialId = material.Material.Id,
                            Position = material.Position
                        });
                }
            }

            if (Entity.Skills != null)
            {
                foreach (var skill in Entity.Skills)
                {
                    _uow.Repository<CourseSkillDBO>()
                        .Create(new CourseSkillDBO()
                        {
                            CourseId = Entity.Id,
                            SkillChange = skill.SkillChange,
                            SkillId = skill.Skill.Id
                        });
                }
            }
        }

        public void Delete(CourseBO Entity)
        {
            Delete(c => c.Id == Entity.Id);
        }

        public void Delete(int Id)
        {
            Delete(c => c.Id == Id);
        }

        public void Delete(Expression<Func<CourseBO, bool>> condition)
        {
            foreach (var course in Get(condition))
            {
                _uow.Repository<CourseDBO>().Delete(course.Id);

                _uow.Repository<CourseMaterialDBO>().Delete(m => m.CourseId == course.Id);

                _uow.Repository<AccountCourse>().Delete(m => m.CourseId == course.Id);
            }
        }

        public CourseBO Get(int Id)
        {
            return Get(c => c.Id == Id).FirstOrDefault();
        }

        public IEnumerable<CourseBO> Get(Expression<Func<CourseBO, bool>> condition)
        {
            var predicate = condition.Compile();

            return _uow.Repository<EducationProject.Core.DAL.CourseDBO>().Get(t => true)
                .Select(c => new CourseBO()
                {
                    Description = c.Description,
                    CreatorId = c.CreatorId,
                    IsVisible = c.IsVisible,
                    Title = c.Title,
                    Id = c.Id,
                    Materials = _uow.Repository<EducationProject.Core.DAL.CourseMaterialDBO>()
                    .Get(b => b.CourseId == c.Id).Select(b => new CourseMaterialBO
                    {
                        Material = _materials.Get(b.MaterialId),
                        Position = b.Position
                    }),
                    Skills = _uow.Repository<EducationProject.Core.DAL.CourseSkillDBO>()
                    .Get(b => b.CourseId == c.Id).Select(b => new CourseSkillBO
                    {
                        Skill = _skills.Get(b.SkillId),
                        SkillChange = b.SkillChange
                    })
                }).Where(c => predicate(c) == true);
        }

        public void Save()
        {
            _uow.Save();
        }

        public void Update(CourseBO Entity)
        {
            Update(Entity, e => e.Id == Entity.Id);
        }

        public void Update(CourseBO Entity, Expression<Func<CourseBO, bool>> Condition)
        {
            foreach(var i in Get(Condition))
            {
                _uow.Repository<EducationProject.Core.DAL.CourseDBO>()
                    .Update(new EducationProject.Core.DAL.CourseDBO()
                    {
                        Description = Entity.Description,
                        CreatorId = Entity.CreatorId,
                        IsVisible = Entity.IsVisible,
                        Id = i.Id,
                        Title = Entity.Title
                    });

                _uow.Repository<EducationProject.Core.DAL.CourseMaterialDBO>().Delete(c => c.CourseId == i.Id);

                foreach(var courseMaterial in Entity.Materials)
                {
                    _uow.Repository<EducationProject.Core.DAL.CourseMaterialDBO>()
                        .Create(new EducationProject.Core.DAL.CourseMaterialDBO()
                        {
                            CourseId = i.Id,
                            MaterialId = courseMaterial.Material.Id,
                            Position = courseMaterial.Position
                        });
                }

                _uow.Repository<EducationProject.Core.DAL.CourseSkillDBO>().Delete(c => c.CourseId == i.Id);

                foreach (var courseSkill in Entity.Skills)
                {
                    _uow.Repository<EducationProject.Core.DAL.CourseSkillDBO>()
                        .Create(new EducationProject.Core.DAL.CourseSkillDBO()
                        {
                            CourseId = i.Id,
                            SkillId = courseSkill.Skill.Id,
                            SkillChange = courseSkill.SkillChange
                        });
                }
            }
        }
    }
}
