using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Server.Shared.Tables.Templates;

public class RowTemplateBase<TItem> : ComponentBase
{
	[Parameter]
	public TItem Context { get; set; } = default!;

	[Parameter]
	public MudTable<TItem> Table { get; set; } = default!;
}