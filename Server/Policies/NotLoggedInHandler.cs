using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Server.Policies;

public class NotLoggedInHandler : AuthorizationHandler<NotLoggedInRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NotLoggedInRequirement requirement)
	{
		if (context.User.FindFirst(c => c.Type == "Name") == null)
		{
			context.Succeed(requirement);
		}
		return Task.CompletedTask;
	}
}
