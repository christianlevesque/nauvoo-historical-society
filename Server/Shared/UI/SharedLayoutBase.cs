using Microsoft.AspNetCore.Components;

namespace Server.Shared.UI;

public class SharedLayoutBase : ComponentBase
{
	protected bool DrawerOpen { get; set; }

	protected void ToggleDrawer() => DrawerOpen = !DrawerOpen;
}