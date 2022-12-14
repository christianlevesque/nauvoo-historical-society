using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Server.Shared.UI.Forms;

public partial class CheckboxAggregator
{
	[Parameter]
	public RenderFragment ChildContent { get; set; } = default!;

	[Parameter]
	public IEnumerable<string> InitialSelected { get; set; } = default!;

	private readonly IList<Checkbox> _inputs = new List<Checkbox>();

	public void AddInput(Checkbox input) => _inputs.Add(input);

	public void RemoveInput(Checkbox input) => _inputs.Remove(input);

	public IEnumerable<string> GetSelected()
	{
		return _inputs.Where(i => i.Checked)
		              .Select(i => i.Value);
	}
}