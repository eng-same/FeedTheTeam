
using Feed.Application.Interfaces;
using Feed.Application.Requests.PoolOption;
using Mediator;

namespace Feed.Application.Commands.PoolOptions;

public class AddPoolOptionCommand : ICommand<int>
{
    public CreatePoolOptionRequest OptionRequest { get; set; } = new();
    public int PoolId { get; set; }
    public string CurrentUserId { get; set; } = string.Empty;
}

public class AddPoolOptionCommandHandler : ICommandHandler<AddPoolOptionCommand, int>
{
    private readonly IPoolOptionService _optionService;

    public AddPoolOptionCommandHandler(IPoolOptionService optionService)
    {
        _optionService = optionService;
    }

    public async ValueTask<int> Handle(AddPoolOptionCommand command, CancellationToken ct)
    {
        return await _optionService.AddOptionAsync(command.OptionRequest, command.PoolId, command.CurrentUserId);
    }
}