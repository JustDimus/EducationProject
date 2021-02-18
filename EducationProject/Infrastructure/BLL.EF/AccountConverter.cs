using EducationProject.BLL.Interfaces;
using EducationProject.Core.DAL.EF;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF.Enums;

namespace Infrastructure.BLL.EF
{
    public class AccountConverter : BaseConverter<AccountDBO, AccountPL>
    {
        private IConverter<CourseDBO, CourseBO> courses;

        private IConverter<BaseMaterialDBO, BaseMaterial> materials;

        public AccountConverter(IMapping<AccountDBO> mapping,
            IConverter<CourseDBO, CourseBO> courseConverter,
            IConverter<BaseMaterialDBO, BaseMaterial> materialConverter)
            : base(mapping)
        {
            this.courses = courseConverter;

            this.materials = materialConverter;
        }

        public override AccountPL Get(AccountDBO entity)
        {
            var accountCourses = entity.AccountCourses.Select(ac => new { ac.Status, course = courses.Get(ac.Course) });

            return new AccountPL()
            {
                Id = entity.Id,
                RegistrationDate = entity.RegistrationDate,
                Email = entity.Email,
                FirstName = entity.FirstName,
                PhoneNumber = entity.PhoneNumber,
                SecondName = entity.SecondName,
                CoursesInProgress = accountCourses.Where(ac => ac.Status == ProgressStatus.InProgress).Select(ac => ac.course),
                PassedCourses = accountCourses.Where(ac => ac.Status == ProgressStatus.Passed).Select(ac => ac.course),
                SkillResults = accountCourses.Where(ac => ac.Status == ProgressStatus.Passed).Select(ac => ac.course)
                .SelectMany(c => c.Skills).GroupBy(c => c.Skill.Id).Select(c => new AccountSkillBO()
                {
                    Skill = c.FirstOrDefault().Skill,
                    CurrentResult = c.Select(f => f.SkillChange).Sum() % c.FirstOrDefault().Skill.MaxValue,
                    Level = c.Select(f => f.SkillChange).Sum() / c.FirstOrDefault().Skill.MaxValue
                })
            };
        }
    }
}
