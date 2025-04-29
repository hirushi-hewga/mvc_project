using FluentValidation;
using mvc_project.Models;

namespace mvc_project.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Назва обов'язова")
                .MaximumLength(100).WithMessage("Максимальна довжина 100 символів");
            
            RuleFor(p => p.Description)
                .MaximumLength(255).WithMessage("Максимальна довжина 255 символів");

            RuleFor(p => p.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Мінімальна ціна не менше 0");
            
            RuleFor(p => p.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Мінімальна кількість не менше 0");
        }
    }
}