using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Text;
using EducationProject.Core.BLL;
using Infrastructure.UOW;
using System.Linq;

namespace Infrastructure.DAL.Mappings
{
    public class AccountMapping : IMapping<AccountBO>
    {
        private UnitOfWork _uow;

        private IMapping<CourseBO> _courses;

        public AccountMapping(UnitOfWork UOW, IMapping<CourseBO> Courses)
        {
            _uow = UOW;
            _courses = Courses;
        }

        public void Create(AccountBO Entity)
        {
            var account = new EducationProject.Core.DAL.AccountDBO()
            {
                Email = Entity.Email,
                FirstName = Entity.FirstName,
                Password = Entity.Password,
                PhoneNumber = Entity.PhoneNumber,
                SecondName = Entity.SecondName,
                RegistrationDate = Entity.RegistrationDate
            };

            _uow.Repository<EducationProject.Core.DAL.AccountDBO>().Create(account);

            Entity.Id = account.Id;

            if (Entity.CoursesInProgress != null)
            {
                foreach (var course in Entity.CoursesInProgress)
                {
                    _uow.Repository<EducationProject.Core.DAL.AccountCourse>()
                        .Create(new EducationProject.Core.DAL.AccountCourse()
                        {
                            AccountId = Entity.Id,
                            CourseId = course.Id,
                            Status = "InProgress"
                        });
                }
            }

            if (Entity.PassedCourses != null)
            {
                foreach (var course in Entity.PassedCourses)
                {
                    _uow.Repository<EducationProject.Core.DAL.AccountCourse>()
                        .Create(new EducationProject.Core.DAL.AccountCourse()
                        {
                            AccountId = Entity.Id,
                            CourseId = course.Id,
                            Status = "Passed"
                        });
                }
            }
        }

        public void Delete(AccountBO Entity)
        {
            Delete(a => a.Id == Entity.Id);
        }

        public void Delete(Predicate<AccountBO> Condition)
        {
            foreach (var entity in Get(Condition))
            {
                _uow.Repository<EducationProject.Core.DAL.AccountCourse>().Delete(t => t.AccountId == entity.Id);

                _uow.Repository<EducationProject.Core.DAL.AccountDBO>().Delete(entity.Id);
            }
        }

        public void Delete(int Id)
        {
            Delete(a => a.Id == Id);
        }

        public IEnumerable<AccountBO> Get(Predicate<AccountBO> Condition)
        {
            return _uow.Repository<EducationProject.Core.DAL.AccountDBO>().Get(t => true)
                .Select(a => 
                {
                    var passed = _courses.Get(c => _uow.Repository<EducationProject.Core.DAL.AccountCourse>()
                    .Get(c => c.AccountId == a.Id && c.Status == "Passed").Select(c => c.CourseId).Contains(c.Id));

                    return new AccountBO()
                    {
                        RegistrationDate = a.RegistrationDate,
                        Email = a.Email,
                        FirstName = a.FirstName,
                        Id = a.Id,
                        Password = a.Password,
                        PhoneNumber = a.PhoneNumber,
                        SecondName = a.SecondName,
                        PassedCourses = passed,
                        CoursesInProgress = _courses.Get(c => _uow.Repository<EducationProject.Core.DAL.AccountCourse>()
                        .Get(c => c.AccountId == a.Id && c.Status == "InProgress").Select(c => c.CourseId).Contains(c.Id)),
                        SkillResults = passed.SelectMany(c => c.Skills).GroupBy(c => c.Skill.Id).Select(c => new AccountSkillBO()
                        {
                            Skill = c.FirstOrDefault().Skill,
                            CurrentResult = c.Select(f => f.SkillChange).Sum() % c.FirstOrDefault().Skill.MaxValue,
                            Level = c.Select(f => f.SkillChange).Sum() / c.FirstOrDefault().Skill.MaxValue
                        })
                    };
                }).Where(a => Condition(a) == true);
        }

        public AccountBO Get(int Id)
        {
            return Get(a => a.Id == Id).FirstOrDefault();
        }

        public void Save()
        {
            _uow.Save();
        }

        public void Update(AccountBO Entity)
        {
            Update(Entity, a => a.Id == Entity.Id);
        }

        public void Update(AccountBO Entity, Predicate<AccountBO> Condition)
        {
            foreach (var entity in Get(Condition))
            {
                _uow.Repository<EducationProject.Core.DAL.AccountDBO>().Update(
                new EducationProject.Core.DAL.AccountDBO()
                {
                    Email = Entity.Email,
                    RegistrationDate = Entity.RegistrationDate,
                    FirstName = Entity.FirstName,
                    Id = entity.Id,
                    Password = Entity.Password,
                    PhoneNumber = Entity.PhoneNumber,
                    SecondName = Entity.SecondName
                });

                _uow.Repository<EducationProject.Core.DAL.AccountCourse>().Delete(c => c.AccountId == entity.Id);

                foreach (var course in Entity.CoursesInProgress)
                {
                    _uow.Repository<EducationProject.Core.DAL.AccountCourse>()
                        .Create(new EducationProject.Core.DAL.AccountCourse()
                        {
                            AccountId = Entity.Id,
                            CourseId = course.Id,
                            Status = "InProgress"
                        });
                }

                foreach (var course in Entity.PassedCourses)
                {
                    _uow.Repository<EducationProject.Core.DAL.AccountCourse>()
                        .Create(new EducationProject.Core.DAL.AccountCourse()
                        {
                            AccountId = Entity.Id,
                            CourseId = course.Id,
                            Status = "Passed"
                        });
                }
            }
        }
    }
}
