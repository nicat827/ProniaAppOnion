﻿using FluentValidation;
using ProniaOnion.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Validators.Category
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryPostDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(50);
        }
    }

}
