using FluentValidation;
using MvcInterface.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcInterface.Models.Validators
{
    public class RegistrationValidator : AbstractValidator<RegisterViewModel>
    {
        public RegistrationValidator()
        {
            this.RuleFor(p => p.Email)
                .NotEmpty()
                .Matches(@"^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$");

            this.RuleFor(p => p.Password)
                .NotEmpty()
                .MinimumLength(4);

            this.RuleFor(p => p.ConfirmPassword)
                .NotEmpty()
                .Equal(p => p.Password);
        }
    }
}
