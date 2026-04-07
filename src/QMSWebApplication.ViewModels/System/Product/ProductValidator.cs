using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Product
{
    public class ProductValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductValidator() {
            RuleFor(p => p.Name)
                .NotNull().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(p => p.AreaId)
                .NotNull().WithMessage("Area Id is required.")
                .GreaterThan(0).WithMessage("Area Id must be a positive integer.");

            RuleFor(p => p.InspPlanId)
                .NotNull().WithMessage("Inspection Plan Id is required.")
                .GreaterThan(0).WithMessage("Inspection Plan Id must be a positive integer.");

            RuleFor(p => p.ModelInternal)
                .MaximumLength(100).WithMessage("Model internal must not exceed 100 characters.");

            RuleFor(p => p.CustomerName)
                .MaximumLength(100).WithMessage("Customer name must not exceed 100 characters.");

            RuleFor(p => p.Notes)
                .MaximumLength(100).WithMessage("Notes must not exceed 100 characters.");

            RuleFor(p => p.Description)
                .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");

            RuleFor(p => p.MoldQuanlity)
                .NotNull().WithMessage("Molde Quantity is required.")
                .GreaterThan(0).WithMessage("Modle Quantity must be a positive integer.");

            RuleFor(p => p.CavityQuanlity)
                .NotNull().WithMessage("Cavity Quantity is required.")
                .GreaterThan(0).WithMessage("Cavity Quantity must be a positive integer.");
        }
    }
}
