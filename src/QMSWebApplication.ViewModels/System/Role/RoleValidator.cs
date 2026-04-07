using FluentValidation;

namespace QMSWebApplication.ViewModels.System.Role
{
    public class RoleValidator: AbstractValidator<RoleCreateRequest>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(256).WithMessage("Role name must not exceed 256 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(512).WithMessage("Description must not exceed 512 characters.");

            RuleFor(x => x.RoleUser)
                .GreaterThanOrEqualTo(0).WithMessage("IntRoleUser must be non-negative.");

            RuleFor(x => x.Level)
                .GreaterThanOrEqualTo(0).WithMessage("IntLevel must be non-negative.");
        }
    }
}
