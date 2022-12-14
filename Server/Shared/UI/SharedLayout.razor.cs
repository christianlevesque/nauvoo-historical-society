using Microsoft.AspNetCore.Components;

namespace Server.Shared.UI;

public partial class SharedLayout
{
	[Parameter]
	public RenderFragment ChildContent { get; set; } = default!;
}