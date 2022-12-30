using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Server.Shared;
using Server.Tools;

namespace Server.Pages.Dashboard.MyAccount.EmailChange;

[Route(Urls.Dashboard.MyAccount.EmailChange.Confirm)]
[Layout(typeof(DashboardNarrowLayout))]
public partial class EmailChangeConfirm
{
	[Parameter]
	[SupplyParameterFromQuery]
	public string UserId { get; set; } = default!;

	[Parameter]
	[SupplyParameterFromQuery]
	public string Email { get; set; } = default!;

	[Parameter]
	[SupplyParameterFromQuery]
	public string Code { get; set; } = default!;

	[CascadingParameter]
	private Task<AuthenticationState> AuthState { get; set; } = default!;

	protected override async Task OnInitializedAsync()
	{
		ErrorMessage = "We are verifying your new email address. Please wait...";

		Model.UserId = UserId;
		Model.NewEmail = Email;
		Model.VerificationCode = Code;

		var authState = await AuthState;
		await SubmitRequest(() => Service.PerformEmailChange(Model, authState.User));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Dashboard.MyAccount.EmailChange.Successful);
		}
	}
}