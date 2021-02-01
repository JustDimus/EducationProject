using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Infrastructure.BLL.Commands
{
    public class AddExistingMaterialToCourseCommand : ICommand
    {
        public string Name => "AddExistingMaterialToCourse";

        private IMapping<CourseBO> _courses;

        private IMapping<BaseMaterial> _materials;

        public AddExistingMaterialToCourseCommand(IMapping<CourseBO> courses, IMapping<BaseMaterial> materials)
        {
            _courses = courses;

            _materials = materials;
        }

        public IOperationResult Handle(object[] Params)
        {
            var account = Params[0] as AccountAuthenticationData;

            int courseId = Convert.ToInt32(Params[1]);

            int materialId = Convert.ToInt32(Params[2]);

            var course = _courses.Get(courseId);

            var material = _materials.Get(materialId);

            if(course.Materials.Any(s => s.Material.Id == materialId))
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Such material '{material.Title}' already exists in this course '{course.Title}'"
                };
            }
            else
            {
                course.Materials = course.Materials.Append(new CourseMaterialBO()
                { 
                    Material = material,
                    Position = course.Materials.Count()
                });
            }

            _courses.Update(course);

            _courses.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Added material '{material.Title}' to course '{course.Title}'"
            };
        }
    }
}
