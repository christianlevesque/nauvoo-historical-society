using Microsoft.AspNetCore.Authorization;

namespace Server.Policies;

public class NotLoggedInRequirement : IAuthorizationRequirement
{
	public const string Name = "RequireAnonymous";
}
