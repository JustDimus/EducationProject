using EducationProject.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Validators
{
    public class ChangeAccountCourseValidator : AbstractValidator<ChangeAccountCourseDTO>
    {
        public ChangeAccountCourseValidator()
        {
            this.RuleFor(e => e.AccountId).GreaterThan(0);

            this.RuleFor(e => e.CourseId).GreaterThan(0);

            this.RuleFor(e => e.Status);
        }
    }
}
