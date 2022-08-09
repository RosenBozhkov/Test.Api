using Business.Models.v1;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.v1;

/// <summary>
/// Fluent validator for <c cref="JobCreateRequest"/>
/// </summary>
public class FluentCreateJobValidator : AbstractValidator<JobCreateRequest>
{
    /// <summary>
    /// Fluent validator constructor for JobRequest
    /// </summary>
    public FluentCreateJobValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(5)
            .WithMessage("Name should be 5 chars at least")
            .MaximumLength(35)
            .WithMessage("Name should be 35 chars at most");
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price cannot be bellow 0");
    }
}
