#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Core.Validation;

namespace Core.Account;

[ExcludeFromCodeCoverage]
public class ResetPasswordDto : HoneypotDto<int>
{
	[Required]
	public string UserId { get; set; }

	[Required]
	public string VerificationCode { get; set; }

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
}