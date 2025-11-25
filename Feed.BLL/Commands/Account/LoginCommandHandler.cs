using Feed.Application.Interfaces;
using Feed.Application.Requests.Account;
using Mediator;
using Microsoft.AspNetCore.Identity;

namespace Feed.Application.Commands.Account;

public class LoginHandler : ICommandHandler<LoginCommand, string>
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

    public async ValueTask<string> Handle(LoginCommand command, CancellationToken ct)
    {
        var user = await _userManager.FindByNameAsync(command.Request.UsernameOrEmail)
                   ?? await _userManager.FindByEmailAsync(command.Request.UsernameOrEmail);

        if (user == null || !user.IsActive)
            throw new ApplicationException("Invalid username/email or inactive user");

        var result = await _signInManager.CheckPasswordSignInAsync(user, command.Request.Password, false);

        if (!result.Succeeded)
            throw new ApplicationException("Invalid password");

        // Return JWT
        return _tokenService.GenerateToken(user);
    }
}

public class LoginCommand: ICommand<string>
{
   public LoginRequest Request { get; set; }
}