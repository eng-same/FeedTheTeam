
using Feed.Application.Commands.Account;
using Feed.Application.Requests.Account;

namespace Feed.PL.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var token = await _mediator.Send(new RegisterCommand() { Request =request});
        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _mediator.Send(new LoginCommand() { Request = request });
        return Ok(new { Token = token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // JWT stateless, front-end can remove token
        return Ok(new { Success = true });
    }
}
