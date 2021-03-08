using EducationProject.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Validators
{
    public class ChangeAccountMaterialValidator : AbstractValidator<ChangeAccountMaterialDTO>
    {
        public ChangeAccountMaterialValidator()
        {
            this.RuleFor(e => e.AccountId).GreaterThan(0);

            this.RuleFor(e => e.MaterialId).GreaterThan(0);
        }
    }
}
