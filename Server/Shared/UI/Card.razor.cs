using Microsoft.AspNetCore.Components;

namespace Server.Shared.UI;

public partial class Card
{
	[Parameter]
	public string? Title { get; set; }

	[Parameter]
	public string HeaderClass { get; set; } = "mud-theme-primary";

	[Parameter]
	public string? Subtitle { get; set; }

	[Parameter]
	public string? Icon { get; set; }

	[Parameter]
	public RenderFragment? CardContent { get; set; }

	[Parameter]
	public RenderFragment? CardActions { get; set; }
}