using System.ComponentModel.DataAnnotations;

namespace Sienar.Validation;

public class RequireLowerAttribute : RegexTestAttributeBase
{
	protected override ValidationResult? IsValid(
		object? value,
		ValidationContext context)
		=> ValidatePatternMatches(value, context, "[a-z]");
}