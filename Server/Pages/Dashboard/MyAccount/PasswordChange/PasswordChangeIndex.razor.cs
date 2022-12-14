using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Server.Shared;
using Server.Tools;

namespace Server.Pages.Dashboard.MyAccount.PasswordChange;

[Route(Urls.Dashboard.MyAccount.PasswordChange.Index)]
[Layout(typeof(DashboardNarrowLayout))]
public partial class PasswordChangeIndex
{
	protected override async Task OnSubmit()
	{
		await SubmitRequest(() => Service.ChangePassword(Model, new ClaimsPrincipal()));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Dashboard.MyAccount.PasswordChange.Successful);
		}
	}
}