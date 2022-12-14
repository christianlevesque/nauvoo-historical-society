using Microsoft.AspNetCore.Components;

namespace Server.Shared.UI;

public partial class AppBar
{
	[Parameter]
	public RenderFragment ChildContent { get; set; } = default!;

	private bool _drawerOpen;

	private void DrawerToggle()
	{
		_drawerOpen = !_drawerOpen;
	}
}