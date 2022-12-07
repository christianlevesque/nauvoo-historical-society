#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.Account;

[ExcludeFromCodeCoverage]
public class PerformEmailChangeDto
{
	[Required]
	[EmailAddress]
	public string NewEmail { get; set; }

	[Required]
	public string UserId { get; set; }

	[Required]
	public string VerificationCode { get; set; }
}