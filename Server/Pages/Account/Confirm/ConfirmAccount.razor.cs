using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Services.Identity;
using Server.Shared.Pages;
using Server.Tools;

namespace Server.Pages.Account.Confirm;

[Route(Urls.Account.Confirm.Index)]
[AllowAnonymous]
public partial class ConfirmAccount : ActionPage<ConfirmAccount, IAccountService, ConfirmAccountDto>
{
	[Parameter]
	[SupplyParameterFromQuery]
	public string UserId { get; set; } = default!;

	[Parameter]
	[SupplyParameterFromQuery]
	public string Code { get; set; } = default!;

	protected override async Task OnParametersSetAsync()
	{
		Model.UserId = UserId;
		Model.VerificationCode = Code;
		await SubmitRequest(() => Service.ConfirmAccount(Model));

		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Account.Confirm.Successful);
		}
	}
}