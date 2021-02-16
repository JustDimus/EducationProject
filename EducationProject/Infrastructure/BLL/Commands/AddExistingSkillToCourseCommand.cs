using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
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

        private IMapping<CourseDBO> courses;

        private IMapping<SkillDBO> skills;

        public AddExistingSkillToCourseCommand(IMapping<CourseDBO> courseMapping, IMapping<SkillDBO> skillMapping)
        {
            this.courses = courseMapping;

            this.skills = skillMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            var account = Params[0] as AccountAuthenticationData;

            int? courseId = Params[1] as int?;

            int? skillId = Params[2] as int?;

            int? skillChange = Params[3] as int?;

            if(courses.Any(c => c.CourseSkills.Any(cs => cs.CourseId == courseId && cs.SkillId == skillId)))
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Such skill 'Id: {skillId}' already exists in this course 'Id: {courseId}'"
                };
            }

            courses.Get(courseId.Value).CourseSkills.Add(new CourseSkillDBO
            {
                CourseId = courseId.Value,
                SkillId = skillId.Value
            });

            courses.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Added skill 'Id: {skillId}' to course 'Id: {courseId}'"
            };
        }
    }
}
