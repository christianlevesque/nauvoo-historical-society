using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Server.Shared;
using Server.Tools;

namespace Server.Pages.Dashboard.MyAccount.EmailChange;

[Route(Urls.Dashboard.MyAccount.EmailChange.Index)]
[Layout(typeof(DashboardNarrowLayout))]
public partial class EmailChangeIndex
{
	protected override async Task OnSubmit()
	{
		await SubmitRequest(() => Service.InitiateEmailChange(Model, new ClaimsPrincipal()));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Dashboard.MyAccount.EmailChange.Requested);
		}
	}
}