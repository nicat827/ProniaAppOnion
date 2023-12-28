using FluentValidation;
using ProniaOnion.Application.Dtos.AppUser;

namespace ProniaOnion.Application.Validators.AppUser
{

    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {
        private const string REQUIRED_MESS = "{PropertyName} is required!";
        private const string MINLENGTH_MESS = "{PropertyName} must be at least {MinLength} characters long!";
        private const string MAXLENGTH_MESS = "{PropertyName} must be less {MaxLength} characters!";
        public LoginDtoValidator()
        {
            RuleFor(a => a.UserNameOrEmail)
              .NotEmpty().WithMessage(REQUIRED_MESS)
              .MaximumLength(256).WithMessage(MAXLENGTH_MESS)
              .MinimumLength(4).WithMessage(MINLENGTH_MESS);
            RuleFor(a => a.Password)
              .NotEmpty().WithMessage(REQUIRED_MESS)
              .MaximumLength(100).WithMessage(MAXLENGTH_MESS)
              .MinimumLength(8).WithMessage(MINLENGTH_MESS);
        }
    }
}
