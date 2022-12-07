using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Client.Services.Infrastructure;

namespace Client.Shared.Tables.Templates.Dashboard;

public partial class StateTemplate
{
	[Inject]
	private IStateService Service { get; set; } = default!;

	private async Task DeleteState(Guid id)
	{
		if (await Service.Delete(id))
		{
			await Table.ReloadServerData();
		}
	}
}