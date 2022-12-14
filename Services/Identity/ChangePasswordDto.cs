#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Services.Validation;

namespace Services.Identity;

[ExcludeFromCodeCoverage]
public class ChangePasswordDto
{
	[Required]
	[DataType(DataType.Password)]
	[RequireUpper(ErrorMessage = "Your new password must contain an uppercase letter")]
	[RequireLower(ErrorMessage = "Your new password must contain a lowercase letter")]
	[RequireDigit(ErrorMessage = "Your new password must contain a number")]
	[RequireSpecial(ErrorMessage = "Your new password must contain a special character")]
	public string NewPassword { get; set; }

	[Required]
	[Compare("NewPassword", ErrorMessage = "The passwords do not match")]
	public string ConfirmNewPassword { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string CurrentPassword { get; set; }
}