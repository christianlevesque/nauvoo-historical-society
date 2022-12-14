using System;
using Microsoft.AspNetCore.Components;
using Server.State;

namespace Server.Shared.UI;

public partial class DashboardSharedLayout : IDisposable
{
	[Inject]
	public LoadingStateProvider LoadingState { get; set; } = default!;

	[Parameter]
	public RenderFragment ChildContent { get; set; } = default!;

	protected override void OnInitialized()
	{
		LoadingState.OnChange += StateHasChanged;
	}

	public void Dispose()
	{
		LoadingState.OnChange -= StateHasChanged;
	}
}