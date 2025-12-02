using Feed.Application.Commands.PoolOptions;
using Feed.Application.Interfaces;
using Feed.Application.Requests.Pool;
using FluentValidation;
using Mediator;

namespace Feed.Application.Commands.Pool;

public class CreatePoolCommand : ICommand<int>
{
    public CreatePoolRequest PoolRequest { get; set; }
    
    public string CurrentUserId { get; set; }
}

public class CreatePoolValidator : AbstractValidator<CreatePoolRequest>
{
    public CreatePoolValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
        RuleFor(x => x.ClosesAt)
            .GreaterThan(DateTime.UtcNow).WithMessage("ClosesAt must be a future date.");
        RuleFor(x => x.Options)
        .NotEmpty().WithMessage("At least one option is required.")
        .Must(options => options != null && options.Count >= 2).WithMessage("At least two options are required.");
        RuleForEach(x => x.Options).SetValidator(new CreatePoolOptionValidator());

    }
}


public class CreatePoolCommandHandler : ICommandHandler<CreatePoolCommand, int>
{
    private readonly IPoolService _poolService;
    private readonly IValidator<CreatePoolRequest> _validator;

    public CreatePoolCommandHandler(IPoolService poolService, IValidator<CreatePoolRequest> validator)
    {
        _poolService = poolService;
        _validator = validator;
    }

    public async ValueTask<int> Handle(CreatePoolCommand command, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(command.PoolRequest, ct);
        return await _poolService.CreatePoolAsync(command.PoolRequest, command.CurrentUserId);
    }
}

