using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Server.Shared.Modals.Users;

public partial class UserChangePassword
{
	[Parameter]
	public string UserId { get; set; } = default!;

	[Parameter]
	public string Username { get; set; } = default!;

	protected override async Task OnSubmit()
	{
		await SubmitRequest(() => Service.UpdateUserPassword(Model));
		if (WasSuccessful)
		{
			SuccessMessage = $"{Username}'s password was updated successfully!";
		}
	}

	protected override void OnInitialized()
	{
		Model.UserId = UserId;
	}
}