using FluentValidation;
using ProniaOnion.Application.Dtos.Color;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Validators.Color
{
    public class ColorCreateDtoValidator : AbstractValidator<ColorPostDto>
    {
        public ColorCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(50);
        }
    }
}
