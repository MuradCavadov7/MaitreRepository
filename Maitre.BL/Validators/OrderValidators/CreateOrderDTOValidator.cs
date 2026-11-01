using FluentValidation;
using Maitre.BL.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.Validators.OrderValidators;

public class CreateOrderDTOValidator : AbstractValidator<CreateOrderDTO>
{
    public CreateOrderDTOValidator()
    {

        RuleForEach(x => x.OrderItems)
            .NotNull()
            .NotEmpty()
            .WithMessage("Order items cannot be empty.")
            .ChildRules(orderItem =>
        {
            orderItem.RuleFor(item => item.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be a positive integer.");
            orderItem.RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be a positive integer.")
                .LessThanOrEqualTo(100).WithMessage("Quantity must be less than or equal to 100.");
        });
    }
}


