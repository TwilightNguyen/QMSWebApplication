using FluentValidation;

namespace QMSWebApplication.ViewModels.System.ProductionPlan
{
    public class ProductionPlanValidator : AbstractValidator<ProductionPlanCreateRequest>
    {
        public ProductionPlanValidator() {
            RuleFor(p => p.LineId)
                .NotNull().WithMessage("Line Id is required.")
                .GreaterThan(0).WithMessage("Line Id must be non-negative.");

            RuleFor(p => p.JobId)
                .NotNull().WithMessage("Job Id is required.")
                .GreaterThan(0).WithMessage("Job Id must be non-negative.");
            
            RuleFor(p => p.Notes)
                .MaximumLength(100).WithMessage("Notes must be exceed 100 characters.");

            //RuleFor(p => p.ProductionDate)
            RuleFor(p => p.PlannedQuantity)
                .NotNull().WithMessage("Planned Quantity is requred.")
                .GreaterThan(0).WithMessage("Planned Quantity must be non-negative.");

            RuleFor(p => p.LotInform)
                .MaximumLength(100).WithMessage("Lot In Form must be exceed 100 characters.");

            RuleFor(p => p.MaterialInform)
                .MaximumLength(50).WithMessage("Material In Form must be exceed 50 characters.");
        }
    }

}
