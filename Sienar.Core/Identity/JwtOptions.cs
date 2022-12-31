#nullable disable

namespace Sienar.Identity;

public class JwtOptions
{
	public string SecurityKey { get; set; }
	public string Issuer { get; set; }
	public string Audience { get; set; }
	public int Expiration { get; set; }
}