﻿using FluentValidation;
using MvcInterface.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcInterface.Models.Validators
{
    public class LogInValidator : AbstractValidator<LogInViewModel>
    {
        public LogInValidator()
        {
            this.RuleFor(p => p.Email)
                .NotEmpty()
                .Matches(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$");

            this.RuleFor(p => p.Password)
                .NotEmpty()
                .MinimumLength(4);
        }
    }
}
