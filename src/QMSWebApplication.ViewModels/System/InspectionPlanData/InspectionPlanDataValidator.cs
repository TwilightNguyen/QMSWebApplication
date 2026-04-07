using FluentValidation;

namespace QMSWebApplication.ViewModels.System.InspectionPlanData
{
    public class InspectionPlanDataValidator : AbstractValidator<InspectionPlanDataCreateRequest>
    {
        public InspectionPlanDataValidator() { 
            RuleFor(RuleFor => RuleFor.InspPlanSubId)
                .NotNull().WithMessage("InspPlanSub Id is required.")
                .GreaterThan(0).WithMessage("InspPlanSubId must be a positive integer.");

            RuleFor(RuleFor => RuleFor.CharacteristicId)
                .NotNull().WithMessage("Characteristic Id is required.")
                .GreaterThan(0).WithMessage("CharacteristicId must be a positive integer.");

            RuleFor(RuleFor => RuleFor.SampleSize)
                .MaximumLength(50).WithMessage("SampleSize must not exceed 50 characters.");

            RuleFor(RuleFor => RuleFor.LSL)
                .LessThanOrEqualTo(RuleFor => RuleFor.USL).WithMessage("LSL must be less than or equal to USL.");

            RuleFor(RuleFor => RuleFor.PercentControlLimit)
                .GreaterThanOrEqualTo(0).WithMessage("PercentControlLimit must be non-negative.");

            RuleFor(RuleFor => RuleFor.CpkMax)
                .GreaterThanOrEqualTo(0).WithMessage("CpkMax must be non-negative.");

            RuleFor(RuleFor => RuleFor.CpkMin)
                .GreaterThanOrEqualTo(0).WithMessage("CpkMin must be non-negative.");

            RuleFor(RuleFor => RuleFor.SPCChart)
                .NotNull().WithMessage("SPCChart is required.");

            RuleFor(RuleFor => RuleFor.DataEntry)
                .NotNull().WithMessage("DataEntry is required.");

            RuleFor(RuleFor => RuleFor.CpkControl)
                .NotNull().WithMessage("SpkControl is required.");
        }
    }
}
