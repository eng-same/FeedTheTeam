
using Feed.Application.Interfaces;
using Mediator;

namespace Feed.Application.Commands.PoolOptions;

public class DeletePoolOptionCommand : ICommand<bool>
{
    public int OptionId { get; set; }
    public string CurrentUserId { get; set; } = string.Empty;
}

public class DeletePoolOptionCommandHandler : ICommandHandler<DeletePoolOptionCommand, bool>
{
    private readonly IPoolOptionService _optionService;

    public DeletePoolOptionCommandHandler(IPoolOptionService optionService)
    {
        _optionService = optionService;
    }

    public async ValueTask<bool> Handle(DeletePoolOptionCommand command, CancellationToken ct)
    {
        await _optionService.DeleteOptionAsync(command.OptionId, command.CurrentUserId);
        return true;
    }
}