using Feed.Application.Commands.Pool;
using Feed.Application.Queries.Pool;
using Feed.Application.Requests.Pool;
using Feed.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Feed.PL.Controllers;


[Route("api/[controller]")]
[ApiController]
public class PoolsController : ControllerBase
{
    private readonly IMediator _mediator;
    

    public PoolsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

    // GET: api/pools
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PoolFilter poolFilter)
    {
        var result = await _mediator.Send(new GetPoolsQuery() { Filter = poolFilter });
        if (result == null) return NotFound();
        return Ok(result);
    }

    // GET: api/pools/{id}
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetPoolByIdQuery() { PoolId = id });
        return result == null ? NotFound() : Ok(result);
    }

    // POST: api/pools
    [HttpPost]
    [Authorize]
	public async Task<IActionResult> Create([FromBody] CreatePoolRequest request)
	{
		var currentUserId = GetCurrentUserId();
		if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();


        if (request == null) return BadRequest("Request body is required");
        if (string.IsNullOrWhiteSpace(request.Title)) return BadRequest("Title is required");
        if (request.Options == null || !request.Options.Any()) return BadRequest("At least one option is required");

        var command = new CreatePoolCommand() { PoolRequest = request, CurrentUserId = currentUserId };
        try
        {
            var createdId = await _mediator.Send(command);
            if (createdId <= 0) return BadRequest("Failed to create pool");
            return CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
        }
        catch (DbUpdateException dbEx)
        {
            //  inner exception message to help debugging
            var msg = dbEx.InnerException?.Message ?? dbEx.Message;
            return BadRequest($"Database update failed: {msg}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/pools/{id}
    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update( int id , [FromBody] UpdatePoolRequest  request)
    {
        var currentUserId = GetCurrentUserId();
       
        var command = new UpdatePoolCommand() { updatePoolRequest = request, CurrentUserId = currentUserId };

        try
        {
            var updated = await _mediator.Send(command);
            return updated ? Ok("Updated Successfully") : NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/pools/{id}
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var currentUserId = GetCurrentUserId();
        
        var command = new DeletePoolCommand() { PoolId = id, CurrentUserId = currentUserId };
        try
        {
            var deleted = await _mediator.Send(command);
            return deleted ? NoContent() : NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/pools/{id}/open
    [HttpPost("{id:int}/open")]
    [Authorize]
    public async Task<IActionResult> Open(int id)
    {
        var currentUserId = GetCurrentUserId();
       
        var command = new OpenPoolCommand() { PoolId = id, CurrentUserId = currentUserId };
        try
        {
            var ok = await _mediator.Send(command);
            return ok ? Ok() : NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/pools/{id}/close
    [HttpPost("{id:int}/close")]
    [Authorize]
    public async Task<IActionResult> Close(int id)
    {
        var currentUserId = GetCurrentUserId();
     
        var command = new ClosePoolCommand() { PoolId = id, CurrentUserId = currentUserId };
        try
        {
            var ok = await _mediator.Send(command);
            return ok ? Ok() : NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}