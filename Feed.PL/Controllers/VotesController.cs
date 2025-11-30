using Feed.Application.Commands.Vote;

using Feed.Application.Queries.Votes;
using Feed.Application.Requests.Vote;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Feed.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VotesController( IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        // POST: api/votes/{poolId}
        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Vote( [FromBody] VoteRequest request)
        {
            var currentUserId = GetCurrentUserId();
            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            try
            {
                var created = _mediator.Send(new AddVoteCommand()
                {
                    Dto =request ,
                    CurrentUserId = currentUserId
                });
                return Created("", created);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/votes/summary?poolId=1
        [HttpGet("summary")]
        public async Task<IActionResult> Summary([FromQuery] int poolId)
        {
            var result = _mediator.Send(new GetVotesByPoolQuery() { PoolId = poolId });
            if (result == null) return NotFound();
            return Ok(result);
            //we need to check if the pool exeist if yes show total of no show not found 
        }
    }
}
