using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Command
{
    public class CommandValidator : AbstractValidator<CommandCreateRequest>
    {
        public CommandValidator() { 
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Command Name is required.")
                .MaximumLength(100).WithMessage("Command Name must not exceed 100 characters.");

            RuleFor(x => x.Notes)
                .MaximumLength(255).WithMessage("Command Notes must not exceed 255 characters.");
        }
    }
}
