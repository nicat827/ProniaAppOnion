﻿
using FluentValidation;
using ProniaOnion.Application.Dtos.Tag;

namespace ProniaOnion.Application.Validators
{
    public class TagCreateDtoValidator:AbstractValidator<TagCreateDto>
    {
        public TagCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);
        }
    }
}