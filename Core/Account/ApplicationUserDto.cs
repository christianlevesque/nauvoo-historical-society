#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Core.Account;

public class ApplicationUserDto
{
	[ExcludeFromCodeCoverage]
	public string Id { get; set; }

	[ExcludeFromCodeCoverage]
	public string UserName { get; set; }

	[ExcludeFromCodeCoverage]
	public string Email { get; set; }

	[ExcludeFromCodeCoverage]
	public bool IsVerified { get; set; }

	public IEnumerable<string> Roles { get; set; }

	public ApplicationUserDto()
	{
		Roles = Array.Empty<string>();
	}
}