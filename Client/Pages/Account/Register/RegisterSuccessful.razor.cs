using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Client.Tools;

namespace Client.Pages.Account.Register;

[Route(Urls.Account.Register.Successful)]
[AllowAnonymous]
public partial class RegisterSuccessful
{
	[Parameter]
	[SupplyParameterFromQuery]
	public string Username { get; set; } = default!;

	[Parameter]
	[SupplyParameterFromQuery]
	public string Email { get; set; } = default!;
}