using EducationProject.Core.BLL;
using EducationProject.DAL.Mappings;
using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.DAL.Mappings
{
    public class CourseMapping : IMapping<Course>
    {
        private UnitOfWork _uow;

        public CourseMapping(UnitOfWork UOW)
        {
            _uow = UOW;
        }

        public void Create(Course Entity)
        {
            var course = new EducationProject.Core.DAL.Course()
            {
                Description = Entity.Description,
                CreatorId = Entity.CreatorId,
                IsVisible = Entity.IsVisible,
                Title = Entity.Title
            };

            _uow.Repository<EducationProject.Core.DAL.Course>().Create(course);

            Entity.Id = course.Id;

            foreach(var material in Entity.Materials)
            {
                _uow.Repository<EducationProject.Core.DAL.CourseMaterial>()
                    .Create(new EducationProject.Core.DAL.CourseMaterial()
                    {
                        CourseId = Entity.Id,
                        MaterialId = material.Material.Id,
                        Position = material.Position
                    });
            }

            foreach (var skill in Entity.Skills)
            {
                _uow.Repository<EducationProject.Core.DAL.CourseSkill>()
                    .Create(new EducationProject.Core.DAL.CourseSkill()
                    {
                        CourseId = Entity.Id,
                        SkillChange = skill.SkillChange,
                        SkillId = skill.Skill.Id
                    });
            }
        }

        public void Delete(Course Entity)
        {
            Delete(c => c.Id == Entity.Id);
        }

        public void Delete(Predicate<Course> Condition)
        {
            foreach(var course in Get(Condition))
            {
                _uow.Repository<EducationProject.Core.DAL.Course>().Delete(course.Id);

                _uow.Repository<EducationProject.Core.DAL.CourseMaterial>().Delete(m => m.CourseId == course.Id);
            }
        }

        public void Delete(int Id)
        {
            Delete(c => c.Id == Id);
        }

        public IEnumerable<Course> Get(Predicate<Course> Condition)
        {
            return _uow.Repository<EducationProject.Core.DAL.Course>().Get(t => true)
                .Select(c => new Course()
                {
                    Description = c.Description,
                    CreatorId = c.CreatorId,
                    IsVisible = c.IsVisible,
                    Title = c.Title,
                    Id = c.Id,
                    Materials = _uow.Repository<EducationProject.Core.DAL.CourseMaterial>()
                    .Get(b => b.CourseId == c.Id).Select(b => new CourseMaterial 
                    { 
                        Material = _uow.Repository<EducationProject.Core.DAL.Material>().Get(b.MaterialId),
                        Position = b.Position
                    }),
                    Skills = _uow.Repository<EducationProject.Core.DAL.CourseSkill>()
                    .Get(b => b.CourseId == c.Id).Select(b => new CourseSkill
                    {
                        Skill = _uow.Repository<EducationProject.Core.DAL.Skill>().Get(b.SkillId),
                        SkillChange = b.SkillChange
                    })
                }).Where(c => Condition(c) == true);
        }

        public Course Get(int Id)
        {
            return Get(c => c.Id == Id).FirstOrDefault();
        }

        public void Save()
        {
            _uow.Save();
        }

        public void Update(Course Entity)
        {
            Update(Entity, e => e.Id == Entity.Id);
        }

        public void Update(Course Entity, Predicate<Course> Condition)
        {
            foreach(var i in Get(Condition))
            {
                _uow.Repository<EducationProject.Core.DAL.Course>()
                    .Update(new EducationProject.Core.DAL.Course()
                    {
                        Description = Entity.Description,
                        CreatorId = Entity.CreatorId,
                        IsVisible = Entity.IsVisible,
                        Id = i.Id,
                        Title = Entity.Title
                    });

                _uow.Repository<EducationProject.Core.DAL.CourseMaterial>().Delete(c => c.CourseId == i.Id);

                foreach(var courseMaterial in i.Materials)
                {
                    _uow.Repository<EducationProject.Core.DAL.CourseMaterial>()
                        .Create(new EducationProject.Core.DAL.CourseMaterial()
                        {
                            CourseId = i.Id,
                            MaterialId = courseMaterial.Material.Id,
                            Position = courseMaterial.Position
                        });
                }

                _uow.Repository<EducationProject.Core.DAL.CourseSkill>().Delete(c => c.CourseId == i.Id);

                foreach (var courseSkill in i.Skills)
                {
                    _uow.Repository<EducationProject.Core.DAL.CourseSkill>()
                        .Create(new EducationProject.Core.DAL.CourseSkill()
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
