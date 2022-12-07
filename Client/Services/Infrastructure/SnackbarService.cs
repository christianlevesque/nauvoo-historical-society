using Autoinjector;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;

namespace Client.Services.Infrastructure;

[Service(ServiceLifetime.Scoped, typeof(ISnackbarService))]
public class SnackbarService : ISnackbarService
{
	private readonly ISnackbar _snackbar;

	public SnackbarService(ISnackbar snackbar)
	{
		_snackbar = snackbar;
	}

	public void Success(string message)
	{
		_snackbar.Add(message, Severity.Success);
	}

	public void Error(string message)
	{
		_snackbar.Add(message, Severity.Error, config => { config.RequireInteraction = true; });
	}
}

public interface ISnackbarService
{
	void Success(string message);
	void Error(string message);
}