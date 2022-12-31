#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Sienar.Validation;

namespace Sienar.Identity;

[ExcludeFromCodeCoverage]
public class RegisterDto : HoneypotDto<int>
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	[StringLength(32, ErrorMessage = "Username must be between 6 and 32 characters", MinimumLength = 6)]
	public string UserName { get; set; }

	[Required]
	[DataType(DataType.Password)]
	[StringLength(64, ErrorMessage = "Password must be between 8 and 64 characters", MinimumLength = 8)]
	[RequireUpper(ErrorMessage = "Your password must contain an uppercase letter")]
	[RequireLower(ErrorMessage = "Your password must contain a lowercase letter")]
	[RequireDigit(ErrorMessage = "Your password must contain a number")]
	[RequireSpecial(ErrorMessage = "Your password must contain a special character")]
	public string Password { get; set; }

	[Required]
	[DataType(DataType.Password)]
	[Compare("Password", ErrorMessage = "The passwords do not match")]
	public string ConfirmPassword { get; set; }

	public bool AcceptTos { get; set; }
}