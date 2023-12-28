
using FluentValidation;
using ProniaOnion.Application.Dtos;

namespace ProniaOnion.Application.Validators.Tag
{
    public class TagCreateDtoValidator : AbstractValidator<TagPostDto>
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
