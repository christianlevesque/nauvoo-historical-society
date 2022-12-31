#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Sienar.Identity;

[ExcludeFromCodeCoverage]
public class InitiateEmailChangeDto
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	[Compare("Email", ErrorMessage = "The email addresses do not match")]
	public string ConfirmEmail { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string ConfirmPassword { get; set; }
}