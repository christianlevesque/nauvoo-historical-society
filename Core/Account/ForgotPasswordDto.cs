#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.Account;

[ExcludeFromCodeCoverage]
public class ForgotPasswordDto : HoneypotDto<int>
{
	[Required]
	public string AccountName { get; set; }
}