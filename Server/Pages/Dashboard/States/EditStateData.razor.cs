using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Server.Shared;
using Server.Tools;
using Services;
using Services.Infrastructure;

namespace Server.Pages.Dashboard.States;

[Layout(typeof(DashboardNarrowLayout))]
[Authorize(Roles = Roles.Admin)]
[Route($"{Urls.Dashboard.States.Edit}/{{stateId:guid}}")]
public partial class EditStateData
{
	[Parameter]
	public Guid StateId { get; set; }

	protected override async Task OnInitializedAsync()
	{
		Model = await SubmitRequest(() => Service.Get(StateId)) ?? new StateDto();
		Model.Id = StateId;
	}

	protected override async Task OnSubmit()
	{
		await SubmitRequest(() => Service.Edit(Model));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Dashboard.States.Index);
		}
	}
}