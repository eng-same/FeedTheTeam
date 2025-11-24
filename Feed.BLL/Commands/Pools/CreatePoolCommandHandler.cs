using Feed.Application.Interfaces;
using Feed.Application.Requests.Pool;
using Mediator;

namespace Feed.Application.Commands.Pool;

public class CreatePoolCommand : ICommand<int>
{
    public CreatePoolRequest PoolRequest { get; set; }
    
    public string CurrentUserId { get; set; }
}
public class CreatePoolCommandHandler : ICommandHandler<CreatePoolCommand, int>
{
    private readonly IPoolService _poolService;

    public CreatePoolCommandHandler(IPoolService poolService)
    {
        _poolService = poolService;
    }

    public async ValueTask<int> Handle(CreatePoolCommand command, CancellationToken ct)
    {
        return await _poolService.CreatePoolAsync(command.PoolRequest, command.CurrentUserId);
    }
}

