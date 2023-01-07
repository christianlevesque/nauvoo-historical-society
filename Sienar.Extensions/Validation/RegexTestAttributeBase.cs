using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Sienar.Validation;

public abstract class RegexTestAttributeBase : ValidationAttribute
{
	/// <summary>
	/// Validates that a value matches the specified regex pattern
	/// </summary>
	/// <param name="value">The value to test</param>
	/// <param name="context">The <see cref="ValidationContext"/></param>
	/// <param name="regex">The regex string to match</param>
	/// <returns></returns>
	protected ValidationResult? ValidatePatternMatches(
		object? value,
		ValidationContext context,
		[RegexPattern] string regex)
	{
		var displayName = context.DisplayName;
		var memberName = new [] { context.MemberName! };

		if (value == null)
		{
			return new ValidationResult($"{displayName} cannot be null", memberName);
		}

		var stringVal = value.ToString()!;
		return Regex.IsMatch(stringVal, regex)
			? ValidationResult.Success
			: new ValidationResult(ErrorMessage, memberName);
	}
}