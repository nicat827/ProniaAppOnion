using FluentValidation;
using ProniaOnion.Application.Dtos;
using ProniaOnion.Application.Dtos.Color;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Validators.Color
{
    public class ColorUpdateDtoValidator : AbstractValidator<ColorPutDto>
    {
        public ColorUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(50);

        }
    }
}
