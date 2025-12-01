

using Feed.Application.Requests.PoolOption;
using FluentValidation;

namespace Feed.Application.Validators.PoolOptions;

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
