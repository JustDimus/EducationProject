using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class IsCourseExistCommand : ICommand
    {
        public string Name => "IsCourseExist";

        private IMapping<CourseBO> _courses;

        public IsCourseExistCommand(IMapping<CourseBO> courses)
        {
            _courses = courses;
        }

        public IOperationResult Handle(object[] Params)
        {
            int courseId = 0;

            Convert.ToInt32(Params[0]);

            if(courseId == 0)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid course data: IsCourseExistCommand"
                };
            }

            if (_courses.Get(courseId) == null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Such course doesn't exist: IsCourseExistCommand"
                };
            }
            else
            {
                return new OperationResult()
                {
                    Status = ResultType.Success,
                    Result = $"Course exists: IsCourseExistCommand"
                };
            }
        }
    }
}
