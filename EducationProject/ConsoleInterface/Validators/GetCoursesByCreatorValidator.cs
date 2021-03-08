using EducationProject.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Validators
{
    public class GetCoursesByCreatorValidator : AbstractValidator<GetCoursesByCreatorDTO>
    {
        public GetCoursesByCreatorValidator(PageInfoValidator pageInfoValidator)
        {
            this.RuleFor(e => e.AccountId).GreaterThan(0);

            this.RuleFor(e => e.PageInfo).NotNull();

            this.RuleFor(e => e.PageInfo).SetValidator(pageInfoValidator);
        }
    }
}
