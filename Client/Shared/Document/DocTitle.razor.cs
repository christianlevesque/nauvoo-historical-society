using Microsoft.AspNetCore.Components;

namespace Client.Shared.Document;

public partial class DocTitle
{
	[Parameter]
	public string Title { get; set; } = default!;
}