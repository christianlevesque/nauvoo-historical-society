using System;
using Microsoft.AspNetCore.Components;
using Services.Identity;
using Server.State;

namespace Server.Shared.UI.Drawers;

public partial class DrawerHeader : IDisposable
{
	[Inject]
	private AccountStateProvider Account { get; set; } = default!;

	private ApplicationUserDto? User => Account.User;

	protected override void OnInitialized()
	{
		Account.OnChange += StateHasChanged;
	}

	public void Dispose()
	{
		Account.OnChange -= StateHasChanged;
	}
}