using EducationProject.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Validators
{
    public class PageInfoValidator : AbstractValidator<PageInfoDTO>
    {
        public PageInfoValidator()
        {
            this.RuleFor(e => e.PageNumber).GreaterThan(0);

            this.RuleFor(e => e.PageSize).GreaterThan(0);
        }
    }
}
