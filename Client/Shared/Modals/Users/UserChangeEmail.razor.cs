using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Shared.Modals.Users;

public partial class UserChangeEmail
{
	[Parameter]
	public string UserId { get; set; } = default!;

	[Parameter]
	public string Username { get; set; } = default!;

	[Parameter]
	public string Email { get; set; } = default!;

	[CascadingParameter]
	private MudDialogInstance Instance { get; set; } = default!;

	protected override async Task OnSubmit()
	{
		if (Model.Email == Email)
		{
			ErrorMessage = "The email address was not changed.";
			return;
		}

		await SubmitRequest(() => Service.UpdateUserEmail(Model));
		if (WasSuccessful)
		{
			SuccessMessage = $"{Username}'s email address was changed successfully! They should receive a verification link shortly.";
		}
	}

	protected override void OnInitialized()
	{
		Model.UserId = UserId;
	}
}