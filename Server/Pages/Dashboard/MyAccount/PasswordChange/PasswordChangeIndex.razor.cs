using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Server.Shared;
using Server.Tools;

namespace Server.Pages.Dashboard.MyAccount.PasswordChange;

[Route(Urls.Dashboard.MyAccount.PasswordChange.Index)]
[Layout(typeof(DashboardNarrowLayout))]
public partial class PasswordChangeIndex
{
	[CascadingParameter]
	private Task<AuthenticationState> AuthState { get; set; } = default!;

	protected override async Task OnSubmit()
	{
		var authState = await AuthState;
		await SubmitRequest(() => Service.ChangePassword(Model, authState.User));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Dashboard.MyAccount.PasswordChange.Successful);
		}
	}
}