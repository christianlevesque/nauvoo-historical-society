using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Services;

namespace Server.Shared.Pages;

public abstract class ActionPage<TPage, TService, TModel> : ActionPage<TPage, TService>
	where TPage : ComponentBase
	where TModel : new()
{
	protected TModel Model = new ();
}

public abstract class ActionPage<TPage, TService> : PersistentStatePage<TPage>
	where TPage : ComponentBase
{
	private int _counter;
	protected string? ErrorMessage;
	protected string? SuccessMessage;

	protected bool Loading => _counter > 0;

	protected bool WasSuccessful { get; set; }

	[Inject]
	protected ILogger<TPage> Logger { get; set; } = default!;

	[Inject]
	protected NavigationManager NavManager { get; set; } = default!;

	[Inject]
	protected TService Service { get; set; } = default!;

	protected async Task<TReturn?> SubmitRequest<TReturn>(Func<Task<ServiceResult<TReturn>>> submitFunc)
	{
		WasSuccessful = false;

		Logger.LogInformation("Submitting service request");

		++_counter;
		var result = await submitFunc();
		--_counter;

		Logger.LogInformation("Service request submitted");

		WasSuccessful = result.WasSuccessful;

		return result.Result;
	}

	protected void ResetError()
	{
		ErrorMessage = null;
	}

	// Not all action or form pages will need this
	// But some do, so we need to inherit PersistentStatePage
	// and create a default implementation of SaveState()
	protected override void SaveState() {}
}