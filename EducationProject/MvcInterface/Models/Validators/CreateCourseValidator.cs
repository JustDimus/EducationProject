using FluentValidation;
using MvcInterface.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcInterface.Models.Validators
{
    public class CreateCourseValidator : AbstractValidator<CreateCourseViewModel>
    {
        public CreateCourseValidator()
        {
            this.RuleFor(c => c.Title)
                .NotEmpty();

            this.RuleFor(c => c.Description)
                .NotEmpty();
        }
    }
}
