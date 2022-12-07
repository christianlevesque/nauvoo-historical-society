using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace Client.Tools;

public static class JwtParser
{
	public static IEnumerable<Claim> ParseClaims(string jwt)
	{
		var claims = new List<Claim>();
		var payload = jwt.Split('.')[1];
		var jsonBytes = ParseBase64WithoutPadding(payload);

		var kvp = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
		if (kvp == null)
		{
			return claims;
		}

		// Handle roles differently, as they may be a string or a string[]
		ParseKeyAsPossibleArray(kvp, claims, ClaimTypes.Role);

		var parsed = kvp.Select(p => new Claim(p.Key, p.Value.ToString()!));

		claims.AddRange(parsed);

		return claims;
	}

	private static void ParseKeyAsPossibleArray(IDictionary<string, object> kvp, List<Claim> claims, string key)
	{
		if (!kvp.TryGetValue(key, out var roleClaim))
		{
			return;
		}

		claims.AddRange(ParseArray(roleClaim.ToString()).Select(c => new Claim(ClaimTypes.Role, c)));
		kvp.Remove(ClaimTypes.Role);
	}

	private static IEnumerable<string> ParseArray(string? input)
	{
		var claims = new List<string>();

		if (string.IsNullOrEmpty(input))
		{
			return claims;
		}

		if (input.StartsWith("["))
		{
			claims.AddRange(JsonSerializer.Deserialize<IEnumerable<string>>(input)!);
		}
		else
		{
			claims.Add(input);
		}

		return claims;
	}

	private static byte[] ParseBase64WithoutPadding(string base64)
	{
		base64 = (base64.Length % 4) switch
		{
			2 => base64 + "==",
			3 => base64 + "=",
			_ => base64
		};

		return Convert.FromBase64String(base64);
	}
}