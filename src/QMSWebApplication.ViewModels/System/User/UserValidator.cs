using FluentValidation;

namespace QMSWebApplication.ViewModels.System.User
{
    public class UserValidator : AbstractValidator<UserCreateRequest>
    {
        public UserValidator() {
            RuleFor(x => x.RoleId)
                .MaximumLength(255).WithMessage("Role Id must not exceed 255 characters.");
            
            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Invalid email address format.")
                .MaximumLength(100).WithMessage("Email address must not exceed 100 characters.");
            
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");
            
            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required.")
                .MaximumLength(100).WithMessage("Department must not exceed 100 characters.");
            
            RuleFor(x => x.StaffId)
                .NotEmpty().WithMessage("Staff ID is required.")
                .MaximumLength(100).WithMessage("Staff ID must not exceed 100 characters.");
            
            RuleFor(x => x.Enable)
                .InclusiveBetween(0, 1).WithMessage("Enable must be either 0 (disabled) or 1 (enabled).");
            
            RuleFor(x => x.SelectedAreaId)
                .MaximumLength(50).WithMessage("Selected Area Id must not exceed 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter (A-Z).")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter (a-z).")
                .Matches(@"\d").WithMessage("Password must contain at least one digit (0-9).")
                .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        }
    }
}
