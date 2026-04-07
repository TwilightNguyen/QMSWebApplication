using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.User
{
    public class UserChangePasswordValidator : AbstractValidator<UserChangePasswordRequest>
    {
        public UserChangePasswordValidator() {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be a positive integer.");
            
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
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
