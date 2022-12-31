#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Sienar.Identity;

[ExcludeFromCodeCoverage]
public class ForgotPasswordDto : HoneypotDto<int>
{
	[Required]
	public string AccountName { get; set; }
}