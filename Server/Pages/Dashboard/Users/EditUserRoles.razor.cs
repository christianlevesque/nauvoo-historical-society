using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Services.Identity;
using Server.Shared;
using Server.Shared.UI.Forms;
using Server.Tools;
using Services;

namespace Server.Pages.Dashboard.Users;

[Layout(typeof(DashboardNarrowLayout))]
[Authorize(Roles = Roles.Admin)]
[Route($"{Urls.Dashboard.Users.Index}/{{userId}}/roles")]
public partial class EditUserRoles
{
	[Parameter]
	public string UserId { get; set; } = default!;

	private CheckboxAggregator _aggregator = default!;

	private ApplicationUserDto? _user;

	protected override async Task OnInitializedAsync()
	{
		_user = await SubmitRequest(() => Service.GetUserData(UserId));
		Model.UserId = UserId;
	}

	protected override async Task OnSubmit()
	{
		Model.Roles = _aggregator.GetSelected();
		await SubmitRequest(() => Service.AddUserToRole(Model));
	}
}