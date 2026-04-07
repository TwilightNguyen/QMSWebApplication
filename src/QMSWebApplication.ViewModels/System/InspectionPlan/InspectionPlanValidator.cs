using FluentValidation;

namespace QMSWebApplication.ViewModels.System.InspectionPlan
{
    public class InspectionPlanValidator : AbstractValidator<InspectionPlanCreateRequest>
    {
        public InspectionPlanValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Inspection Plan Name is required.")
                .MaximumLength(100).WithMessage("Inspection Plan Name must not exceed 100 characters.");

            RuleFor(x => x.AreaId)
                .GreaterThan(0).WithMessage("Area Id must be a positive integer.");
        }
    }
}
