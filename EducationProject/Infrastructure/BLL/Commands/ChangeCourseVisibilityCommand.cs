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
    public class ChangeCourseVisibilityCommand : ICommand
    {
        public string Name => "ChangeCourseVisibility";

        private IMapping<CourseDBO> courses;

        public ChangeCourseVisibilityCommand(IMapping<CourseDBO> courseMapping)
        {
            courses = courseMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            AccountAuthenticationData account = Params[0] as AccountAuthenticationData;

            int? courseId = Params[1] as int?;

            bool? newState = Params[2] as bool?;

            if(account is null || courseId is null || newState.HasValue == false)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data: ChangeCourseVisibilityCommand"
                };
            }
            
            if(courses.Any(c => c.CourseMaterials.Any()) == false)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Course must have at least 1 material for publication"
                };
            }

            courses.Get(courseId.GetValueOrDefault()).IsVisible = newState.GetValueOrDefault();

            courses.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Succesfully changed course visibility state to: {newState.Value}"
            };
        }
    }
}
