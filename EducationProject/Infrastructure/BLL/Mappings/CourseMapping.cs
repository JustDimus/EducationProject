using EducationProject.BLL.DTO;
using EducationProject.BLL.Interfaces;
using EducationProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.BLL.Mappings
{
    public class CourseMapping : IMapping<Course, ShortCourseInfoDTO>
    {
        public Expression<Func<Course, ShortCourseInfoDTO>> ConvertExpression
        {
            get => c => new ShortCourseInfoDTO()
            {
                Id = c.Id,
                Description = c.Description,
                Title = c.Title,
                CreatorId = c.CreatorId,
                IsVisible = c.IsVisible
            };
        }

        public Course Map(ShortCourseInfoDTO externalEntity)
        {
            return new Course()
            {
                Id = externalEntity.Id,
                Description = externalEntity.Description,
                Title = externalEntity.Title,
                CreatorId = externalEntity.CreatorId,
                IsVisible = externalEntity.IsVisible
            };
        }
    }
}
