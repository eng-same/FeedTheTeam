
using Feed.Application.Requests.Account;
using FluentValidation;

namespace Feed.Application.Validators.Login;

public class RegisterValidator: AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.")
        .Must(_ => HasUpperCase()).WithMessage("Password must contain at least one uppercase letter.")
        .Must(_ => HasLowerCase()).WithMessage("Password must contain at least one lowercase letter.")
        .Must(_ => HasDigit()).WithMessage("Password must contain at least one digit.")
        .Must(_ => HasSpecialCharacter()).WithMessage("Password must contain at least one special character.");
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
    }

    private bool HasUpperCase()
    {
        throw new NotImplementedException();
    }
    private bool HasLowerCase()
    {
        throw new NotImplementedException();
    }
    private bool HasDigit()
    {
        throw new NotImplementedException();
    }
    private bool HasSpecialCharacter()
    {
        throw new NotImplementedException();
    }
}
