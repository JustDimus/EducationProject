using EducationProject.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Validators
{
    public class ChangeEntityValidator<TEntity> : AbstractValidator<ChangeEntityDTO<TEntity>>
    {
        public ChangeEntityValidator()
        {
            this.RuleFor(e => e.AccountId).GreaterThan(0);

            this.RuleFor(e => e.Entity).NotNull();
        }
    }
}
