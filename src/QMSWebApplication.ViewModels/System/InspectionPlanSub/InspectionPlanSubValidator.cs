using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.InspectionPlanSub
{
    public class InspectionPlanSubValidator : AbstractValidator<InspectionPlanSubCreateRequest>
    {
        public InspectionPlanSubValidator()
        {
            RuleFor(x => x.InspPlanId).GreaterThan(0).WithMessage("InspPlan Id must be greater than 0.");
            RuleFor(x => x.PlanTypeId).GreaterThan(0).WithMessage("PlanType Id must be greater than 0.");
        }
    }
}
