using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Core.Account;
using Client.Services;
using Client.Shared;
using Client.Shared.Pages;
using Client.Tools;

namespace Client.Pages.Dashboard.MyAccount.EmailChange;

[Route(Urls.Dashboard.MyAccount.EmailChange.Index)]
[Layout(typeof(DashboardNarrowLayout))]
public partial class EmailChangeIndex
{
	protected override async Task OnSubmit()
	{
		await SubmitRequest(() => Service.InitiateEmailChange(Model));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Dashboard.MyAccount.EmailChange.Requested);
		}
	}
}