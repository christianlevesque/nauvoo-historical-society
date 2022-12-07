using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Repositories.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Services.Identity;

public class SignInService : SignInManager<ApplicationUser>
{
	private readonly JwtOptions _jwtOptions;
	public SignInService(UserService userService,
	                     IHttpContextAccessor contextAccessor,
	                     IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
	                     IOptions<IdentityOptions> optionsAccessor,
	                     ILogger<SignInManager<ApplicationUser>> logger,
	                     IAuthenticationSchemeProvider schemes,
	                     IUserConfirmation<ApplicationUser> confirmation,
	                     IOptions<JwtOptions> jwtOptions)
		: base(userService,
		       contextAccessor,
		       claimsFactory,
		       optionsAccessor,
		       logger,
		       schemes,
		       confirmation)
	{
		_jwtOptions = jwtOptions.Value;
	}

	public async Task<string> SignInApiUser(ApplicationUser user)
	{
		var credentials = GetSigningCredentials();
		var claims = await GetClaims(user);
		var options = GenerateTokenOptions(credentials, claims);
		return new JwtSecurityTokenHandler().WriteToken(options);
	}

	public SigningCredentials GetSigningCredentials()
	{
		var key = Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey);
		var secret = new SymmetricSecurityKey(key);
		return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
	}

	public async Task<IEnumerable<Claim>> GetClaims(ApplicationUser user)
	{
		var claims = new List<Claim>
		{
			new (ClaimTypes.NameIdentifier, user.Id),
			new (ClaimTypes.Email, user.Email),
			new (ClaimTypes.Name, user.UserName)
		};

		var roles = await UserManager.GetRolesAsync(user);
		if (roles == null)
		{
			return claims;
		}

		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		return claims;
	}

	public JwtSecurityToken GenerateTokenOptions(SigningCredentials credentials, IEnumerable<Claim> claims)
	{
		return new JwtSecurityToken(issuer: _jwtOptions.Issuer,
		                            audience: _jwtOptions.Audience,
		                            claims: claims,
		                            signingCredentials: credentials,
		                            expires: DateTime.Now.AddDays(_jwtOptions.Expiration));
	}
}
