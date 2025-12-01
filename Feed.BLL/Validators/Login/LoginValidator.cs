

using Feed.Application.Requests.Account;
using FluentValidation;

namespace Feed.Application.Validators.Login;

public class LoginValidator: AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage("Username or Email is required.")
            .MaximumLength(100).WithMessage("Username or Email cannot exceed 100 characters.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");

    }
}
