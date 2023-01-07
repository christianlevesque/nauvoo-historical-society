using System.ComponentModel.DataAnnotations;

namespace Sienar.Validation;

public class RequireSpecialAttribute : RegexTestAttributeBase
{
	protected override ValidationResult? IsValid(
		object? value,
		ValidationContext context)
		=> ValidatePatternMatches(value, context, @"\W");
}