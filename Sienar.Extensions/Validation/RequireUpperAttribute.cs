using System.ComponentModel.DataAnnotations;

namespace Sienar.Validation;

public class RequireUpperAttribute : RegexTestAttributeBase
{
	protected override ValidationResult? IsValid(
		object? value,
		ValidationContext context)
		=> ValidatePatternMatches(value, context, "[A-Z]");
}