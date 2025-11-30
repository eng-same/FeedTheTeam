using Feed.Application.Commands.PoolOptions;
using Feed.Application.Queries.PoolOptions;
using Feed.Application.Queries.Pool;
using Feed.Application.Requests.PoolOption;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Feed.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoolOptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PoolOptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        // GET: api/pooloptions?poolId=1
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int poolId)
        {
            var result = await _mediator.Send(new GetOptionsByPoolQuery() { PoolId = poolId });
            if (result == null) return NotFound();
            return Ok(result);
        }

        // GET: api/pooloptions/{poolId}/{id}
        [HttpGet("{poolId:int}/{id:int}")]
        public async Task<IActionResult> GetById(int poolId, int id)
        {
            var option = await _mediator.Send(new GetOptionByIdQuery() { OptionId = id });
            if (option == null) return NotFound();
            return Ok(option);
        }

        // POST: api/pooloptions/{poolId}
        [HttpPost("{poolId:int}")]
        [Authorize]
        public async Task<IActionResult> Create(int poolId, [FromBody] CreatePoolOptionRequest request)
        {
            var currentUserId = GetCurrentUserId();
          
            var command = new AddPoolOptionCommand() { OptionRequest = request, PoolId = poolId, CurrentUserId = currentUserId };
            try
            {
                var created = await _mediator.Send(command);
                if (created is int createdId && createdId <= 0) return BadRequest("Failed to create option");
                return Created("", created);
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

        // PUT: api/pooloptions/{poolId}/{id}
        [HttpPut("{poolId:int}/{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int poolId, int id, [FromBody] UpdatePoolOptionDto request)
        {
            var currentUserId = GetCurrentUserId();
            
            var command = new UpdatePoolOptionCommand() { OptionRequest = request, CurrentUserId = currentUserId };
            try
            {
                var updated = (bool) await _mediator.Send(command);
                return updated ? NoContent() : NotFound();
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

        // DELETE: api/pooloptions/{poolId}/{id}
        [HttpDelete("{poolId:int}/{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int poolId, int id)
        {
            var currentUserId = GetCurrentUserId();
           
            var command = new DeletePoolOptionCommand() { OptionId = id, CurrentUserId = currentUserId };
            try
            {
                bool deleted = (bool) await _mediator.Send(command);
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
    }
}
