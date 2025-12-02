
using Feed.Application.Interfaces;
using Feed.Application.Requests.PoolOption;
using FluentValidation;
using Mediator;

namespace Feed.Application.Commands.PoolOptions;

public class AddPoolOptionCommand : ICommand<int>
{
    public CreatePoolOptionRequest OptionRequest { get; set; } = new();
    public int PoolId { get; set; }
    public string CurrentUserId { get; set; } = string.Empty;
}

public class CreatePoolOptionValidator : AbstractValidator<CreatePoolOptionRequest>
{
    public CreatePoolOptionValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Option name is required.")
            .MaximumLength(100).WithMessage("Option name cannot exceed 100 characters.");
        RuleFor(x => x.OptionText)
            .NotEmpty().WithMessage("Option text is required.")
            .MaximumLength(500).WithMessage("Option text cannot exceed 500 characters.");
    }
}

public class AddPoolOptionCommandHandler : ICommandHandler<AddPoolOptionCommand, int>
{
    private readonly IPoolOptionService _optionService;
    private readonly IValidator<CreatePoolOptionRequest> _validator;

    public AddPoolOptionCommandHandler(IPoolOptionService optionService, IValidator<CreatePoolOptionRequest> validator)
    {
        _optionService = optionService;
        _validator = validator;

    }

    public async ValueTask<int> Handle(AddPoolOptionCommand command, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(command.OptionRequest, ct);
        return await _optionService.AddOptionAsync(command.OptionRequest, command.PoolId, command.CurrentUserId);
    }
}
