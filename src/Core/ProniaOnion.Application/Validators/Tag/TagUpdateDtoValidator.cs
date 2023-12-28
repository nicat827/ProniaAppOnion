using FluentValidation;
using ProniaOnion.Application.Dtos;


namespace ProniaOnion.Application.Validators.Tag
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
