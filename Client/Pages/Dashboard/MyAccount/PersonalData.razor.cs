using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MudBlazor;
using Client.Services;
using Client.Shared;
using Client.Shared.Modals.MyAccount;
using Client.Tools;

namespace Client.Pages.Dashboard.MyAccount;

[Route(Urls.Dashboard.MyAccount.PersonalData)]
[Layout(typeof(DashboardNarrowLayout))]
public partial class PersonalData
{
	[Inject]
	private IAccountService Service { get; set; } = default!;

	[Inject]
	private ILogger<PersonalData> Logger { get; set; } = default!;

	[Inject]
	private IJSRuntime Interop { get; set; } = default!;

	[Inject]
	private IDialogService Dialog { get; set; } = default!;

	private bool _isLoading;

	private async Task DownloadPersonalData()
	{
		if (_isLoading)
		{
			return;
		}

		Logger.LogInformation("Downloading personal data");

		_isLoading = true;
		await using var data = await Service.GetPersonalData();
		using var dataWrapper = new DotNetStreamReference(data);
		await Interop.InvokeVoidAsync("downloadFileFromStream", "PersonalData.json", dataWrapper);
		_isLoading = false;
	}

	private void LaunchDeleteAccountDialog()
	{
		Dialog.Show<DeleteAccountConfirmation>();
	}
}