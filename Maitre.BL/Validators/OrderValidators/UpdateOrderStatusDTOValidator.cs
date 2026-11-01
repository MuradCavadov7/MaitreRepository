using FluentValidation;
using Maitre.BL.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.Validators.OrderValidators;

public class UpdateOrderStatusDTOValidator : AbstractValidator<UpdateOrderStatusDTO>
{
    public UpdateOrderStatusDTOValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("wrong status value");
    }
}
