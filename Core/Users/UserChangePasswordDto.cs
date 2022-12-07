#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Core.Validation;

namespace Core.Users;

[ExcludeFromCodeCoverage]
public class UserChangePasswordDto
{
	[Required]
	public string UserId { get; set; }

	[Required]
	[DataType(DataType.Password)]
	[StringLength(64, ErrorMessage = "The user's password must be between 8 and 64 characters long", MinimumLength = 8)]
	[RequireLower(ErrorMessage = "The user's password must contain a lowercase letter")]
	[RequireUpper(ErrorMessage = "The user's password must contain an uppercase letter")]
	[RequireDigit(ErrorMessage = "The user's password must contain a number")]
	[RequireSpecial(ErrorMessage = "The user's password must contain a special character")]
	public string NewPassword { get; set; }

	[Required]
	[Compare("NewPassword", ErrorMessage = "The passwords do not match")]
	public string ConfirmNewPassword { get; set; }
}