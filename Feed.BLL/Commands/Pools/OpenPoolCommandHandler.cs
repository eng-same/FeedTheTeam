using Feed.Application.Interfaces;
using Mediator;
namespace Feed.Application.Commands.Pool;

public class OpenPoolCommand : ICommand<bool>
{ 
    public int PoolId { get; set; }
    public string CurrentUserId { get; set; }

}
public class OpenPoolCommandHandler : ICommandHandler<OpenPoolCommand, bool>
{
    private readonly IPoolService _poolService;

    public OpenPoolCommandHandler(IPoolService poolService)
    {
        _poolService = poolService;
    }

    public async ValueTask<bool> Handle(OpenPoolCommand command, CancellationToken ct)
    {
        await _poolService.OpenPoolAsync(command.PoolId, command.CurrentUserId);
        return true;
    }
}