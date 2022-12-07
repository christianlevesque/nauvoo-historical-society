using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Client.Tools;
using Client.Tools.Extensions;

namespace Client.Shared;

public partial class UnauthorizedRedirect
{
	[Inject]
	private NavigationManager NavManager { get; set; } = default!;

	[CascadingParameter]
	private Task<AuthenticationState> AuthState { get; set; } = default!;

	private bool _showMessage;

	protected override async Task OnInitializedAsync()
	{
		if (!(await AuthState).IsAuthenticated())
		{
			var currentUri = NavManager.ToBaseRelativePath(NavManager.Uri);
			NavManager.NavigateTo($"{Urls.Account.Login}?returnUri=/{currentUri}");
			return;
		}

		_showMessage = true;
	}
}