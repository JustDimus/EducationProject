using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using EducationProject.Core.DAL.EF;

namespace Infrastructure.BLL.Commands
{
    public class AddExistingMaterialToCourseCommand : ICommand
    {
        public string Name => "AddExistingMaterialToCourse";

        private IMapping<CourseDBO> courses;

        private IMapping<BaseMaterialDBO> materials;

        public AddExistingMaterialToCourseCommand(IMapping<CourseDBO> courseMapping, IMapping<BaseMaterialDBO> materialMapping)
        {
            this.courses = courseMapping;

            this.materials = materialMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            var account = Params[0] as AccountAuthenticationData;

            int? courseId = Params[1] as int?;

            int? materialId = Params[2] as int?;

            if(account is null || courseId.HasValue == false || materialId.HasValue == false)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data: AddExistingMaterialToCourseCommand"
                };
            }

            if(courses.Any(c => c.CourseMaterials.Any(cm => cm.CourseID == courseId && cm.MaterialId == materialId)))
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Such material 'Id: {materialId}' already exists in this course 'Id: {courseId}'"
                };
            }

            courses.Get(courseId.Value).CourseMaterials.Add(new CourseMaterialDBO()
            { 
                CourseID = courseId.Value,
                MaterialId = materialId.Value
            });

            courses.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Added material 'Id: {materialId}' to course 'Id: {courseId}'"
            };
        }
    }
}
