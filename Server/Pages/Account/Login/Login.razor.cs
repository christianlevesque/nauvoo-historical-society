using Microsoft.AspNetCore.Components;
using Services.Identity;

namespace Server.Pages.Account.Login;

public partial class Login
{
	[Parameter]
	public string? ReturnUri { get; set; }

	[Parameter]
	public LoginDto Model { get; set; } = default!;

	[Parameter]
	public string? ErrorMessage { get; set; }

	// protected override async Task OnSubmit()
	// {
	// 	SetFormCompletionTime(Model);
	//
	// 	await SubmitRequest(() => Service.Login(Model));
	// 	if (WasSuccessful)
	// 	{
	// 		NavManager.NavigateTo(ReturnUri ?? Urls.Dashboard.Index);
	// 	}
	// }
}