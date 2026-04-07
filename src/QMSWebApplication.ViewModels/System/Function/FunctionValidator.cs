using FluentValidation;

namespace QMSWebApplication.ViewModels.System.Function
{
    public class FunctionValidator : AbstractValidator<FunctionCreateRequest>
    {
        public FunctionValidator() { 
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Function Name is required.")
                .MaximumLength(100).WithMessage("Function Name must not exceed 100 characters.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Function Url is required.")
                .MaximumLength(100).WithMessage("Function Url must not exceed 100 characters.");
        }
    }
}
