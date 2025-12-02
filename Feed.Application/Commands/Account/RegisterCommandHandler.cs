

using Feed.Application.DTOs.Account;
using Feed.Application.Interfaces;
using Feed.Application.Requests.Account;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Identity;


namespace Feed.Application.Commands.Account;

public class RegisterHandler : ICommandHandler<RegisterCommand, UserDto>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IValidator<RegisterRequest> _validator ;

    public RegisterHandler(UserManager<User> userManager, ITokenService tokenService, IValidator<RegisterRequest> validator)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _validator = validator;
    }

    public async ValueTask<UserDto> Handle(RegisterCommand command, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(command.Request ,ct);

        var user = new User
        {
            UserName = command.Request.Username,
            Email = command.Request.Email,
            FirstName = command.Request.FirstName,
            LastName = command.Request.LastName,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, command.Request.Password);

        if (!result.Succeeded)
            throw new ApplicationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        // Create token
        var token = _tokenService.GenerateToken(user);

        // Return DTO
        return new UserDto
        {
            Token = token,
            UserName = user.UserName,
            Id = user.Id
        };
    }
}

public class RegisterValidator : AbstractValidator<RegisterRequest>
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
        .Must(HasUpperCase).WithMessage("Password must contain at least one uppercase letter.")
        .Must(HasLowerCase).WithMessage("Password must contain at least one lowercase letter.")
        .Must(HasDigit).WithMessage("Password must contain at least one digit.")
        .Must(HasSpecialCharacter).WithMessage("Password must contain at least one special character.");
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
    }

    private bool HasUpperCase(string password)
    {
        return password.Any(char.IsUpper);
    }
    private bool HasLowerCase(string password)
    {
        return password.Any(char.IsLower);
    }
    private bool HasDigit(string password)
    {
        return password.Any(char.IsDigit);
    }
    private bool HasSpecialCharacter(string password)
    {
        return password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}

public class RegisterCommand :ICommand <UserDto>
{
    public RegisterRequest Request { get; set; }
}   
