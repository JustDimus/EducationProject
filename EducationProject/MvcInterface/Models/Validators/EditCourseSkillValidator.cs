using FluentValidation;
using MvcInterface.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcInterface.Models.Validators
{
    public class EditCourseSkillValidator : AbstractValidator<EditCourseSkillViewModel>
    {
        public EditCourseSkillValidator()
        {
            this.RuleFor(e => e.CourseId)
                .GreaterThan(0);

            this.RuleFor(e => e.SkillId)
                .GreaterThan(0);

            this.RuleFor(e => e.ChangeValue)
                .GreaterThan(0)
                .WithMessage("Значение должно быть больше нуля");
        }
    }
}
