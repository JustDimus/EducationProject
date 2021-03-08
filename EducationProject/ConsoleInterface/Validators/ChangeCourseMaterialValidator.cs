using EducationProject.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Validators
{
    public class ChangeCourseMaterialValidator : AbstractValidator<ChangeCourseMaterialDTO>
    {
        public ChangeCourseMaterialValidator()
        {
            this.RuleFor(e => e.AccountId).GreaterThan(0);

            this.RuleFor(e => e.CourseId).GreaterThan(0);

            this.RuleFor(e => e.MaterialId).GreaterThan(0);
        }
    }
}
