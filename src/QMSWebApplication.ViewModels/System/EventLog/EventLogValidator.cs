using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.EventLog
{
    public class EventLogValidator : AbstractValidator<EventLogCreateRequest>
    {
        public EventLogValidator()
        {
            RuleFor(x => x.Event)
                .NotEmpty().WithMessage("Event is required.")
                .MaximumLength(200).WithMessage("Event must not exceed 200 characters.");
        }
    }
}
