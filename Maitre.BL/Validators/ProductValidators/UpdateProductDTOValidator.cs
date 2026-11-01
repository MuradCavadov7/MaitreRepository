using FluentValidation;
using Maitre.BL.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.Validators.ProductValidators;

public class UpdateProductDTOValidator : AbstractValidator<UpdateProductDTO>
{
    public UpdateProductDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(100)
            .WithMessage("Product name cannot exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .PrecisionScale(18, 2, false)
            .WithMessage("Product price must be greater than zero.");

        RuleFor(x => x.Category)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product category is required.")
            .MaximumLength(50)
            .WithMessage("Product category cannot exceed 50 characters.");

    }
}
