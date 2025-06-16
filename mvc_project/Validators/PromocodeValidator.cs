using FluentValidation;
using mvc_project.Models;

namespace mvc_project.Validators
{
    public class PromocodeValidator : AbstractValidator<Promocode>
    {
        public PromocodeValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Назва обов'язкова")
                .MaximumLength(20).WithMessage("Максимальна довжина 20 символів");
            RuleFor(p => p.Discount)
                .NotEmpty().WithMessage("Знижка обов'язкова")
                .InclusiveBetween(0, 100).WithMessage("Знижка має бути від 0 до 100");
        }
    }
}