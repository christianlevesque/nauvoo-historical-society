using Microsoft.AspNetCore.Components;

namespace Client.Shared.Tables;

public partial class TableToolbar
{
	private string _search = string.Empty;

	[Parameter]
	public string Search
	{
		get => _search;
		set
		{
			if (_search == value)
			{
				return;
			}

			_search = value;
			SearchChanged.InvokeAsync(_search);
		}
	}

	[Parameter]
	public string Title { get; set; } = default!;

	[Parameter]
	public EventCallback<string> SearchChanged { get; set; }

	[Parameter]
	public bool HideSearch { get; set; }

	[Parameter]
	public RenderFragment? ChildContent { get; set; }
}