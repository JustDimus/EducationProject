using FluentValidation;
using MvcInterface.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcInterface.Models.Validators
{
    public class EditAccountInfoValidator : AbstractValidator<EditAccountInfoViewModel>
    {
        public EditAccountInfoValidator()
        {
            this.RuleFor(a => a.Id).NotEmpty();

            this.RuleFor(a => a.Email).NotEmpty();
        }
    }
}
