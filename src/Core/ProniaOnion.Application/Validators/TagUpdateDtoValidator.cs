using FluentValidation;
using ProniaOnion.Application.Dtos;


namespace ProniaOnion.Application.Validators
{
    public class TagUpdateDtoValidator : AbstractValidator<TagPostDto>
    {
        public TagUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

        }
    }
}
