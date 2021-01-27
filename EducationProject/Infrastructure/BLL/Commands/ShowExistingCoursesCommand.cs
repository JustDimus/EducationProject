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
    public class ShowExistingCoursesCommand: ICommand
    {
        public string Name => "ShowExistingCourses";

        private IMapping<CourseBO> _courses;

        public ShowExistingCoursesCommand(IMapping<CourseBO> courses)
        {
            _courses = courses;
        }

        public IOperationResult Handle(object[] Params)
        {
            Predicate<CourseBO> condition = Params[0] as Predicate<CourseBO>;

            var coursesData = _courses.Get(t => true);

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = condition is null ? coursesData : coursesData.Where(c => condition(c) == true)
            };
        }
    }
}
