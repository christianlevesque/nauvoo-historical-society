using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Tools.Extensions;

public static class AuthenticationStateExtensions
{
	public static bool IsAuthenticated(this AuthenticationState authState) => authState.User.Identity?.IsAuthenticated ?? false;
}