using Microsoft.AspNetCore.Components;

namespace Server.Shared.UI.Content;

public partial class SpaceBetweenContent
{
	[Parameter]
	public RenderFragment ChildContent { get; set; } = default!;
}