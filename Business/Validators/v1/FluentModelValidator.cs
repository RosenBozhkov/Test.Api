using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using FluentValidation;

namespace Business.Validators.v1;

/// <summary>
/// Fluent validator for <c cref="ModelRequest"/>
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class FluentModelValidator : AbstractValidator<ModelRequest>
{
    /// <summary>
    /// Fluent validator constructor for ModelRequest
    /// </summary>
    public FluentModelValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(5)
            .WithMessage("Name should be 5 chars at least")
            .MaximumLength(25)
            .WithMessage("Name should be 25 chars at most");
    }
}
