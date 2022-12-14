using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Server.Shared;
using Server.Tools;
using Services;

namespace Server.Pages.Dashboard.States;

[Route(Urls.Dashboard.States.Add)]
[Layout(typeof(DashboardNarrowLayout))]
[Authorize(Roles = Roles.Admin)]
public partial class AddState
{
	protected override async Task OnSubmit()
	{
		await SubmitRequest(() => Service.Add(Model));
		if (WasSuccessful)
		{
			Reset();
		}
	}
}