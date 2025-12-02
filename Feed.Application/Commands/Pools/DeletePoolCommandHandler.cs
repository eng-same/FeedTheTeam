

using Feed.Application.Interfaces;
using Mediator;

namespace Feed.Application.Commands.Pool;

public class DeletePoolCommand : ICommand<bool>
{ 
    public int PoolId { get; set; }
    public string CurrentUserId { get; set; }
}

public class DeletePoolCommandHandler : ICommandHandler<DeletePoolCommand, bool>
{
    private readonly IPoolService _poolService;

    public DeletePoolCommandHandler(IPoolService poolService)
    {
        _poolService = poolService;
    }

    public async ValueTask<bool> Handle(DeletePoolCommand command, CancellationToken ct)
    {
        await _poolService.DeletePoolAsync(command.PoolId, command.CurrentUserId);
        return true;
    }
}
