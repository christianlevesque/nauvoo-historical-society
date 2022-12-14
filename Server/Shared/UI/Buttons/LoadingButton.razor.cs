using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Server.Shared.UI.Buttons;

public partial class LoadingButton
{
	[Parameter]
	public bool IsLoading { get; set; }

	[Parameter]
	public RenderFragment ChildContent { get; set; } = default!;

	[Parameter]
	public Size Size { get; set; } = Size.Small;

	[Parameter]
	public Action? OnClick { get; set; }
}