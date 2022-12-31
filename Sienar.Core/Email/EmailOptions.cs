#nullable disable
using System.Diagnostics.CodeAnalysis;

namespace Sienar.Email;

[ExcludeFromCodeCoverage]
public class EmailOptions
{
	public string ApiKey { get; set; }
	public string EmailFromAddress { get; set; }
	public string EmailFromName { get; set; }
	public string ApplicationUrl { get; set; }
	public string EmailSignature { get; set; }
}
