using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using FluentValidation;

namespace Business.Validators.v1;

/// <summary>
/// Fluent validator for <c cref="VisitFinishRequest"/>
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class FluentVisitFinishRequest : AbstractValidator<VisitFinishRequest>
{
    /// <summary>
    /// Fluent validator constructor for VisitFinishRequest
    /// </summary>
    public FluentVisitFinishRequest()
    {
        RuleFor(x => x.AdditionalCost)
          .GreaterThan(0)
          .WithMessage("Price cannot be bellow 0");
    }
}
