using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Core.Validation;

public class RequireLowerAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		var displayName = validationContext.DisplayName;
		var memberName = new [] { validationContext.MemberName! };

		if (value == null)
		{
			return new ValidationResult($"{displayName} cannot be null", memberName);
		}

		var stringVal = value.ToString();
		return Regex.IsMatch(stringVal!, "[a-z]") ? ValidationResult.Success : new ValidationResult(ErrorMessage, memberName);
	}
}