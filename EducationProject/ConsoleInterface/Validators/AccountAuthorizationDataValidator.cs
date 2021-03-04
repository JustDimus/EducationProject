using EducationProject.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Validators
{
    public class AccountAuthorizationDataValidator : AbstractValidator<AccountAuthorizationDataDTO>
    {
        public AccountAuthorizationDataValidator()
        {
            this.RuleFor(e => e.Email).NotEmpty();

            this.RuleFor(e => e.Password).NotEmpty();
        }
    }
}
