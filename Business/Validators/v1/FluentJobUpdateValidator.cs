using Business.Models.v1;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.v1;

/// <summary>
/// Fluent validator for <c cref="JobUpdateRequest"/>
/// </summary>
public class FluentJobUpdateValidator : AbstractValidator<JobUpdateRequest>
{
    /// <summary>
    /// Fluent validator constructor for JobRequest
    /// </summary>
    public FluentJobUpdateValidator()
    {
        RuleFor(x => x.Price)
          .GreaterThan(0)
          .WithMessage("Price cannot be bellow 0");
    }
}