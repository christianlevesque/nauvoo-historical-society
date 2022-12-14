#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Services.Identity;

[ExcludeFromCodeCoverage]
public class ConfirmAccountDto
{
	[Required]
	public string UserId { get; set; }

	[Required]
	public string VerificationCode { get; set; }
}