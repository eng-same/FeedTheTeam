
using Feed.Application.Commands.Account;
using Feed.Application.Requests.Account;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

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
        try
        {
            var userDto = await _mediator.Send(new RegisterCommand { Request = request });

            return Ok(userDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var userDto = await _mediator.Send(new LoginCommand { Request = request });

            return Ok(userDto);
        }
        catch (ApplicationException)
        {
            return BadRequest(new { Message = "Invalid credentials" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // JWT stateless, front-end can remove token
        return Ok(new { Success = true });
    }
}
