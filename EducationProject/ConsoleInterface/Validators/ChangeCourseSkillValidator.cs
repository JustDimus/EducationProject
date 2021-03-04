using EducationProject.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Validators
{
    public class ChangeCourseSkillValidator : AbstractValidator<ChangeCourseSkillDTO>
    {
        public ChangeCourseSkillValidator()
        {
            this.RuleFor(e => e.AccountId).GreaterThan(0);

            this.RuleFor(e => e.Change).GreaterThan(0);

            this.RuleFor(e => e.CourseId).GreaterThan(0);

            this.RuleFor(e => e.SkillId).GreaterThan(0);
        }
    }
}
