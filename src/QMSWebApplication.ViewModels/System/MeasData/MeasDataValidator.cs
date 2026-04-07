using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.MeasData
{
    public class MeasDataValidator : AbstractValidator<MeasDataCreateRequest>
    {
        public MeasDataValidator() { 
            RuleFor(x => x.ProductionId)
                .NotEmpty().WithMessage("Production Id must be required.")
                .GreaterThan(0).WithMessage("Production Id must be a positive interger.");

            RuleFor(x => x.PlanTypeId)
                .NotEmpty().WithMessage("Plan Type Id must be required.")
                .GreaterThan(0).WithMessage("Plan Type Id must be a positive interger.");

            RuleFor(x => x.CavityId)
                .NotEmpty().WithMessage("Cavity Id must be required.")
                .GreaterThan(0).WithMessage("Cavity Id must be a positive interger.");

            RuleFor(x => x.MoldId)
                .NotEmpty().WithMessage("Mold Id must be required.")
                .GreaterThan(0).WithMessage("Mold Id must be a positive interger.");

            RuleFor(x => x.SampleQunality)
                .NotEmpty().WithMessage("Sample quantity must be required.")
                .GreaterThan(0).WithMessage("Sample quantity must be a positive interger.");

            RuleFor(x => x.CharacteristicRange)
                .MaximumLength(64).WithMessage("Characterisitc Range must not exceed 64 character.");

            RuleFor(x => x.Notes)
               .MaximumLength(50).WithMessage("Notes must not exceed 50 character.");

            RuleFor(x => x.Values)
                .NotEmpty().WithMessage("Values must be requred.");
        }
    }
}
