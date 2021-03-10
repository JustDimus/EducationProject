using EducationProject.BLL.DTO;
using EducationProject.BLL.Interfaces;
using EducationProject.Core.Models;
using EducationProject.Core.Models.Enums;
using System;
using System.Linq.Expressions;

namespace EducationProject.Infrastructure.BLL.Mappings
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

        public string ConvertCourseStatus(ProgressStatus status)
        {
            switch (status)
            {
                case ProgressStatus.InProgress:
                    return "InProgress";
                case ProgressStatus.None:
                    return "None";
                case ProgressStatus.Passed:
                    return "Passed";
                default:
                    return "Error";
            }
        }

        public Expression<Func<CourseSkill, CourseSkillDTO>> CourseSkillConvertExpression
        {
            get => cs => new CourseSkillDTO()
            {
                SkillChange = cs.Change,
                SkillId = cs.SkillId,
                CourseId = cs.CourseId
            };
        }
    }
}
