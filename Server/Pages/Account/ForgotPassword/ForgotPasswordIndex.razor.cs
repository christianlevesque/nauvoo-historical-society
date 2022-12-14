using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Server.Tools;

namespace Server.Pages.Account.ForgotPassword;

[Route(Urls.Account.ForgotPassword.Index)]
[AllowAnonymous]
public partial class ForgotPasswordIndex
{
	protected override async Task OnSubmit()
	{
		SetFormCompletionTime(Model);

		await SubmitRequest(() => Service.ForgotPassword(Model));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Account.ForgotPassword.Successful);
		}
	}
}