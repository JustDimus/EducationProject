using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class AddExistingSkillToCourseCommand: ICommand
    {
        public string Name => "AddExistingSkillToCourse";

        private IMapping<CourseBO> _courses;

        private IMapping<SkillBO> _skills;

        public AddExistingSkillToCourseCommand(IMapping<CourseBO> courses, IMapping<SkillBO> skills)
        {
            _courses = courses;

            _skills = skills;
        }

        public IOperationResult Handle(object[] Params)
        {
            var account = Params[0] as AccountAuthenticationData;

            int courseId = Convert.ToInt32(Params[1]);

            int skillId = Convert.ToInt32(Params[2]);

            int skillChange = Convert.ToInt32(Params[3]);

            var course = _courses.Get(courseId);

            var skill = _skills.Get(skillId);

            if (course.Skills.Any(s => s.Skill.Id == skillId))
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Such skill '{skill.Title}' already exists in this course '{course.Title}'"
                };
            }
            else
            {
                var rt = course.Skills.ToList();
                course.Skills = course.Skills.Append(new CourseSkillBO()
                {
                    Skill = skill,
                    SkillChange = skillChange
                });
                var ttytr = course.Skills.ToList();

                Console.WriteLine();
            }

            _courses.Update(course);

            _courses.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Added skill '{skill.Title}' to course '{course.Title}'"
            };
        }
    }
}
