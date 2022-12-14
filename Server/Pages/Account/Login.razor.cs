using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Services.Identity;
using Server.Shared.Pages;
using Server.Tools;

namespace Server.Pages.Account;

[Route(Urls.Account.Login)]
[AllowAnonymous]
public partial class Login : FormPage<Login, IAccountService, LoginDto>
{
	[Parameter]
	[SupplyParameterFromQuery]
	public string? ReturnUri { get; set; }

	protected override async Task OnSubmit()
	{
		SetFormCompletionTime(Model);

		await SubmitRequest(() => Service.Login(Model));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(ReturnUri ?? Urls.Dashboard.Index);
		}
	}
}