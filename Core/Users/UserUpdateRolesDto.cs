#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.Users;

[ExcludeFromCodeCoverage]
public class UserUpdateRolesDto
{
	[Required]
	public string UserId { get; set; }

	public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
}