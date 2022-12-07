using Microsoft.AspNetCore.Components;

namespace Client.Shared.UI.Content;

public partial class UpdatableValueContent
{
	[Parameter]
	public string Name { get; set; } = default!;

	[Parameter]
	public string Value { get; set; } = default!;

	[Parameter]
	public string ButtonText { get; set; } = "Update";

	[Parameter]
	public EventCallback OnClick { get; set; }

	[Parameter]
	public string? Url { get; set; }
}