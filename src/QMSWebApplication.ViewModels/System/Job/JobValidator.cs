using FluentValidation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Job
{
    public class JobValidator : AbstractValidator<JobCreateRequest>
    {
        public JobValidator() {
            RuleFor(j => j.AreaId)
                .GreaterThan(0).WithMessage("Area Id must be a positive interger.");

            RuleFor(j => j.ProductId)
                .GreaterThan(0).WithMessage("Product Id must be a positive interger.");

            RuleFor(j => j.JobCode)
                .NotEmpty().WithMessage("Job Code is required.")
                .MaximumLength(100).WithMessage("Job Code must not exceed 100 character.");

            RuleFor(j => j.POCode)
                .MaximumLength(100).WithMessage("PO Code must not exceed 100 character.");

            RuleFor(j => j.SOCode)
                .MaximumLength(100).WithMessage("SO Code must not exceed 100 character.");

            RuleFor(j => j.PlannedQuantity)
                .GreaterThan(0).WithMessage("Planned quantity must be a positive interger.");
        }
    }
}
