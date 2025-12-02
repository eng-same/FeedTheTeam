using Feed.Application.Requests.Vote;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feed.Application.Validators.Vote
{
    public class VoteRequestValidator : AbstractValidator<VoteRequest>
    {
        public VoteRequestValidator()
        {
            RuleFor(x => x.PoolId)
                .GreaterThan(0).WithMessage("PollId must be a positive integer.");
            RuleFor(x => x.OptionId)
                .GreaterThan(0).WithMessage("OptionId must be a positive integer.");
        }
    }
}
