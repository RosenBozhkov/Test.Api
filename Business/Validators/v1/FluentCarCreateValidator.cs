using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using FluentValidation;

namespace Business.Validators.v1;

/// <summary>
/// Fluent validator for <c cref="CarCreateRequest"/>
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class FluentCarCreateValidator : AbstractValidator<CarCreateRequest>
{
    /// <summary>
    /// Fluent validator constructor for CarRequest
    /// </summary>
    public FluentCarCreateValidator()
    {
        RuleFor(x => x.YearOfCreation)
            .ExclusiveBetween(1930, 2024)
            .WithMessage("Fluent error for invalid year of creation");
        RuleFor(x => x.MakeName)
            .MinimumLength(5)
            .WithMessage("Name should be 5 chars at least")
            .MaximumLength(15)
            .WithMessage("Name should be 15 chars at most");
        RuleFor(x => x.ModelName)
            .MinimumLength(5)
            .WithMessage("Name should be 5 chars at least")
            .MaximumLength(25)
            .WithMessage("Name should be 25 chars at most");
        RuleFor(x => x.Modifier)
            .LessThan(3)
            .WithMessage("Can not modify price more than 3 times the normal one")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Can not make price cheaper than normal for any car");
    }
}