using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using FluentValidation;

namespace Business.Validators.v1;

/// <summary>
/// Fluent validator for <c cref="MakeRequest"/>
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class FluentMakeValidator : AbstractValidator<MakeRequest>
{
    /// <summary>
    /// Fluent validator constructor for MakeRequest
    /// </summary>
    public FluentMakeValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(5)
            .WithMessage("Name should be 5 chars at least")
            .MaximumLength(15)
            .WithMessage("Name should be 15 chars at most");
    }
}
