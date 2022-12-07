using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Shared.UI.Forms;

public class Checkbox : MudCheckBox<bool>, IDisposable
{
	[Parameter]
	public string Value { get; set; } = default!;

	[CascadingParameter]
	public CheckboxAggregator Aggregator { get; set; } = default!;

	protected override void OnInitialized()
	{
		if (string.IsNullOrEmpty(Label))
		{
			Label = Value;
		}

		Aggregator.AddInput(this);

		if (Aggregator.InitialSelected.Contains(Value))
		{
			Checked = true;
		}
	}

	public void Dispose()
	{
		Aggregator.RemoveInput(this);
		GC.SuppressFinalize(this);
	}
}