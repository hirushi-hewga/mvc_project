using FluentValidation;
using mvc_project.Models;

namespace mvc_project.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Назва обов'язкова")
                .MaximumLength(100).WithMessage("Максимальна довжина 100 символів");
        }
    }
}