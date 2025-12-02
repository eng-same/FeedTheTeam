using Feed.Application.DTOs.Account;
using Feed.Application.Interfaces;
using Feed.Application.Requests.Account;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Identity;

namespace Feed.Application.Commands.Account;

public class LoginHandler : ICommandHandler<LoginCommand, UserDto>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IValidator<LoginRequest> _validator;

    public LoginHandler(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, IValidator<LoginRequest> validator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _validator = validator;
    }

    public async ValueTask<UserDto> Handle(LoginCommand command, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(command.Request, ct);

        var user = await _userManager.FindByNameAsync(command.Request.UsernameOrEmail)
             ?? await _userManager.FindByEmailAsync(command.Request.UsernameOrEmail);

        if (user == null || !user.IsActive ||
            !(await _signInManager.CheckPasswordSignInAsync(user, command.Request.Password, false))
                .Succeeded)
        {
            throw new ApplicationException("Invalid credentials");
        }

        var token = _tokenService.GenerateToken(user);

        return new UserDto
        {
            Token = token,
            UserName = user.UserName,
            Id = user.Id
        };
    }
}

public class LoginValidator : AbstractValidator<LoginRequest>
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

public class LoginCommand: ICommand<UserDto>
{
   public LoginRequest Request { get; set; }
}