﻿using EducationProject.BLL.DTO;
using EducationProject.BLL.Interfaces;
using EducationProject.Core.Models;
using System;
using System.Linq.Expressions;

namespace EducationProject.Infrastructure.BLL.Mappings
{
    public class SkillMapping : IMapping<Skill, SkillDTO>
    {
        public Expression<Func<Skill, SkillDTO>> ConvertExpression
        {
            get => s => new SkillDTO()
            {
                Id = s.Id,
                Description = s.Description,
                MaxValue = s.MaxValue,
                Title = s.Title
            };
        }

        public Skill Map(SkillDTO externalEntity)
        {
            return new Skill()
            {
                Id = externalEntity.Id,
                Description = externalEntity.Description,
                MaxValue = externalEntity.MaxValue,
                Title = externalEntity.Title
            };
        }

        public Expression<Func<CourseSkill, CourseSkillDTO>> CourseSkillConvertExpression
        {
            get => cs => new CourseSkillDTO()
            {
                SkillChange = cs.Change,
                SkillId = cs.SkillId,
                SkillTitle = cs.Skill.Title
            };
        }
    }
}
