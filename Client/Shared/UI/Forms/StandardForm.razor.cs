using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace Client.Shared.UI.Forms;

public partial class StandardForm<TModel>
{
	[Parameter]
	public string? FormErrorMessage { get; set; }

	[Parameter]
	public EventCallback<MouseEventArgs> OnReset { get; set; }

	[Parameter]
	public RenderFragment? MoreActions { get; set; }

	[Parameter]
	public RenderFragment? Information { get; set; }

	[Parameter]
	public string Title { get; set; } = default!;

	[Parameter]
	public string SubmitText { get; set; } = default!;

	[Parameter]
	public string ResetText { get; set; } = "Reset";

	[Parameter]
	public TModel Model { get; set; } = default!;

	[Parameter]
	public bool ShowReset { get; set; }

	[Parameter]
	public Func<Task> OnSubmit { get; set; } = default!;

	[Parameter]
	public RenderFragment Fields { get; set; } = default!;

	private bool _loading;

	private async Task HandleSubmit()
	{
		_loading = true;
		await OnSubmit();
		_loading = false;
	}
}