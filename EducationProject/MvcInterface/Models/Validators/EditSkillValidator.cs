using FluentValidation;
using MvcInterface.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcInterface.Models.Validators
{
    public class EditSkillValidator : AbstractValidator<EditSkillViewModel>
    {
        public EditSkillValidator()
        {
            this.RuleFor(s => s.Id).NotEmpty();

            this.RuleFor(s => s.Title)
                .NotEmpty()
                .WithMessage("Название не должно быть пустым");

            this.RuleFor(s => s.MaxValue)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Максимально значение скилла должно быть больше 0");
        }
    }
}
