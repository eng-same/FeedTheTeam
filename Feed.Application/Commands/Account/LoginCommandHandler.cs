using Feed.Application.DTOs.Account;
using Feed.Application.Interfaces;
using Feed.Application.Requests.Account;
using Mediator;
using Microsoft.AspNetCore.Identity;

namespace Feed.Application.Commands.Account;

public class LoginHandler : ICommandHandler<LoginCommand, UserDto>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public LoginHandler(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async ValueTask<UserDto> Handle(LoginCommand command, CancellationToken ct)
    {
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

public class LoginCommand: ICommand<UserDto>
{
   public LoginRequest Request { get; set; }
}