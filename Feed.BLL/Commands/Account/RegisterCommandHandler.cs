

using Feed.Application.Interfaces;
using Feed.Application.Requests.Account;
using Mediator;
using Microsoft.AspNetCore.Identity;


namespace Feed.Application.Commands.Account;

public class RegisterHandler : ICommandHandler<RegisterCommand, string>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;

    public RegisterHandler(UserManager<User> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async ValueTask<string> Handle(RegisterCommand command, CancellationToken ct)
    {
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

        // Generate JWT
        return _tokenService.GenerateToken(user);
    }
}

public class RegisterCommand :ICommand <string>
{
    public RegisterRequest Request { get; set; }
}   
