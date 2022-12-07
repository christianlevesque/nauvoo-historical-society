using System;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Components;

namespace Client.Shared.Pages;

public abstract class FormPage<TPage, TService, TModel> : ActionPage<TPage, TService, TModel>
	where TPage : ComponentBase
	where TModel : new()
{
	protected readonly DateTime FormStarted = DateTime.Now;

	protected abstract Task OnSubmit();

	protected void Reset()
	{
		Model = new TModel();
		ResetError();
	}

	protected void SetFormCompletionTime<TKey>(HoneypotDto<TKey> honeypot) where TKey : struct, IEquatable<TKey>
	{
		honeypot.TimeToComplete = DateTime.Now - FormStarted;
	}
}