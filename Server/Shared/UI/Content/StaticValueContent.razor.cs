using Microsoft.AspNetCore.Components;

namespace Server.Shared.UI.Content;

public partial class StaticValueContent
{
	[Parameter]
	public string Name { get; set; } = default!;

	[Parameter]
	public string Value { get; set; } = default!;
}