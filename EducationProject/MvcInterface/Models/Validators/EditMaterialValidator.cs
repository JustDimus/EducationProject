using FluentValidation;
using MvcInterface.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcInterface.Models.Validators
{
    public class EditMaterialValidator : AbstractValidator<EditMaterialViewModel>
    {
        public EditMaterialValidator()
        {
            this.RuleFor(m => m.Id)
                .GreaterThan(0);

            this.RuleFor(m => m.Title)
                .NotEmpty();

            this.RuleFor(m => m.Type)
                .NotEmpty();

            this.RuleFor(m => m.URI)
                .NotEmpty()
                .When(m => m.Type != "Book");

            this.RuleFor(m => m.Duration)
                .GreaterThan(0)
                .When(m => m.Type == "Video");

            this.RuleFor(m => m.PublicationDate)
                .NotEmpty()
                .When(m => m.Type == "Article");

            this.RuleFor(m => m.Quality)
                .GreaterThan(0)
                .When(m => m.Type == "Video");

            this.RuleFor(m => m.Pages)
                .GreaterThan(0)
                .When(m => m.Type == "Book");
        }
    }
}
