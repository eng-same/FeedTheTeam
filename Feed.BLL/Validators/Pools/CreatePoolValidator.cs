using Feed.Application.Requests.Pool;
using Feed.Application.Validators.PoolOptions;
using FluentValidation;

namespace Feed.Application.Validators.Pools;

public class CreatePoolValidator :AbstractValidator<CreatePoolRequest>
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
