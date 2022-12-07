#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.Account;

[ExcludeFromCodeCoverage]
public class LoginDto : HoneypotDto<int>
{
	[Required]
	public string AccountName { get; set; }

	[Required]
	public string Password { get; set; }
}