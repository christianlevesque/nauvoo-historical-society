using Microsoft.AspNetCore.Components;

namespace Client.Shared.UI;

public partial class SharedLayout
{
	[Parameter]
	public RenderFragment ChildContent { get; set; } = default!;
}