using EducationProject.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Validators
{
    public class CourseVisibilityValidator : AbstractValidator<CourseVisibilityDTO>
    {
        public CourseVisibilityValidator()
        {
            this.RuleFor(e => e.AccountId).GreaterThan(0);

            this.RuleFor(e => e.CourseId).GreaterThan(0);
        }
    }
}
