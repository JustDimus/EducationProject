using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using XMLDataContext.DataContext;
using XMLDataContext.DataSets;
using EducationProject.Core.BLL;
using EducationProject.BLL.Interfaces;
using EducationProject.DAL.Mappings;
using Infrastructure.DAL.Mappings;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            //  var uow = new UnitOfWork();

            ICommandHandler handler = null;

            var t = new XMLDataContext.DataContext.XMLContext();

            UnitOfWork work = new UnitOfWork(t);

            IMapping<Course> courses = new CourseMapping(work);

            IMapping<Account> accounts = new AccountMapping(work, courses);

            //accounts.Delete(t => true);

            var tt = accounts.Get(t => true).ToList();

            var firstAccount = tt.FirstOrDefault();

            var rrwefds = firstAccount.CoursesInProgress.ToList();

            var cc = courses.Get(t => true).FirstOrDefault();

            var sad = firstAccount.CoursesInProgress.ToList();

            sad.Add(courses.Get(t => true).FirstOrDefault());

            firstAccount.CoursesInProgress = sad;

            firstAccount.PassedCourses = new List<Course>();

            var rrwefds1 = firstAccount.CoursesInProgress.ToList();

            accounts.Update(firstAccount);

            var ttpassed = tt.Select(trr => trr.PassedCourses.ToList()).ToList();

            var ttskills = tt.Select(ttrr => ttrr.SkillResults.ToList()).ToList();

           // var ccmaterials = cc.Select(cc => cc.Materials.First().Material).First();
           /* var skill = new EducationProject.Core.DAL.Skill()
            {
                MaxValue = 100,
                Title = "Working"
            };

            work.Repository<EducationProject.Core.DAL.Skill>().Create(skill);

            var material = new EducationProject.Core.DAL.Material()
            {
                Title = "Work_1",
                Description = "It's working",
                Type = "Text"
            };

            work.Repository<EducationProject.Core.DAL.Material>().Create(material);

            CourseSkill courseSkill = new CourseSkill()
            {
                Skill = skill,
                SkillChange = 150
            };

            Course course = new Course()
            {
                Description = "TestCourse",
                IsVisible = false,
                Materials = new List<CourseMaterial>() { new CourseMaterial() { Material = material, Position = 1 } },
                CreatorId = 0,
                Skills = new List<CourseSkill>() { courseSkill },
                Title = "Hello"
            };

            courses.Create(course);
           */
            /*Account account = new Account()
            {
                RegistrationDate = DateTime.Now,
                CoursesInProgress = null,
                Email = "helloWorld",
                FirstName = "Jojo",
                Password = "dsadsa",
                PhoneNumber = "+1111",
                SecondName = "Name"
            };*/

            //accounts.Create(account);

            courses.Save();

            accounts.Save();
            /*
            t.Accounts.Create(new Account()
            {
                Email = "dssadds",
                FirstName = "dsad"
            });

            t.AccountSkills.Create(new AccountSkills()
            {
                CurrentResult = 15,
                Id = 1,
                Level = 2,
                Skill = 123
            });

            t.AccountSkills.Delete(t => true);

            //t.Accounts.Delete(t => true);

            t.Accounts.Save();
            */
            t.Save();
        }
    }
}
