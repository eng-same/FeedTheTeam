
using Feed.Application.Interfaces;
using Feed.Application.Requests.PoolOption;
using Mediator;

namespace Feed.Application.Commands.PoolOptions;

public class UpdatePoolOptionCommand : ICommand<bool>
{
    public UpdatePoolOptionDto OptionRequest { get; set; } = new();
    public string CurrentUserId { get; set; } = string.Empty;
}

public class UpdatePoolOptionCommandHandler : ICommandHandler<UpdatePoolOptionCommand, bool>
{
    private readonly IPoolOptionService _optionService;

    public UpdatePoolOptionCommandHandler(IPoolOptionService optionService)
    {
        _optionService = optionService;
    }

    public async ValueTask<bool> Handle(UpdatePoolOptionCommand command, CancellationToken ct)
    {
        await _optionService.UpdateOptionAsync(command.OptionRequest, command.CurrentUserId);
        return true;
    }
}