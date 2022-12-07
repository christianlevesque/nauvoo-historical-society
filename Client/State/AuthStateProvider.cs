using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Autoinjector;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Client.Services;
using Client.Services.Infrastructure;
using Client.Tools;

namespace Client.State;

[Service(ServiceLifetime.Scoped, typeof(AuthenticationStateProvider))]
public class AuthStateProvider : AuthenticationStateProvider
{
	private readonly HttpClient _client;
	private readonly ITokenStorageService _tokenService;
	private readonly AuthenticationState _anon;
	private readonly ILogger<AuthStateProvider> _logger;

	public AuthStateProvider(HttpClient client, ITokenStorageService tokenService, ILogger<AuthStateProvider> logger)
	{
		_client = client;
		_tokenService = tokenService;
		_logger = logger;
		_anon = GetAuthStateFromJwt();
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var token = await _tokenService.GetToken();
		if (string.IsNullOrWhiteSpace(token))
		{
			_logger.LogInformation("User is not logged in");
			return _anon;
		}

		_logger.LogInformation("User was logged in. Setting BEARER token on HttpClient");
		_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.AuthenticationScheme, token);

		return GetAuthStateFromJwt(token);
	}

	public async Task NotifyUserAuthentication(string jwt)
	{
		await _tokenService.SetToken(jwt);
		var authState = GetAuthStateFromJwt(jwt);
		NotifyAuthenticationStateChanged(Task.FromResult(authState));
	}

	public async Task NotifyUserLogout()
	{
		await _tokenService.UnsetToken();
		NotifyAuthenticationStateChanged(Task.FromResult(_anon));
	}

	private AuthenticationState GetAuthStateFromJwt(string jwt = "")
	{
		var claims = string.IsNullOrEmpty(jwt)
			             ? Array.Empty<Claim>()
			             : JwtParser.ParseClaims(jwt);
		foreach (var claim in claims)
		{
			_logger.LogInformation("Claim: {type} = {value}", claim.Type, claim.Value);
		}

		return CreateAuthState(claims);
	}

	private AuthenticationState CreateAuthState(IEnumerable<Claim> claims)
	{
		ClaimsIdentity identity;
		if (claims.Any())
		{
			identity = new ClaimsIdentity(claims, Constants.JwtAuthType);
		}
		else
		{
			identity = new ClaimsIdentity();
		}

		var principal = new ClaimsPrincipal(identity);
		return new AuthenticationState(principal);
	}
}