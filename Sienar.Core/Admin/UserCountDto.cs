using System.Diagnostics.CodeAnalysis;

namespace Sienar.Admin;

[ExcludeFromCodeCoverage]
public class UserCountDto
{
	public int Count { get; set; }

	public UserCountDto() {}

	public UserCountDto(int count)
	{
		Count = count;
	}
}