using System;
using Autoinjector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Client.State;

[Service(ServiceLifetime.Scoped)]
public class LoadingStateProvider
{
	private ILogger<LoadingStateProvider> _logger;
	private int _loadingCounter;

	private int LoadingCounter
	{
		get => _loadingCounter;
		set
		{
			_loadingCounter = value;
			_logger.LogInformation("There are currently {count} loading operations", value);
			NotifyStateChanged();
		}
	}

	public bool Loading => LoadingCounter > 0;

	public event Action? OnChange;

	public LoadingStateProvider(ILogger<LoadingStateProvider> logger)
	{
		_logger = logger;
	}

	private void NotifyStateChanged() => OnChange?.Invoke();

	public void AddLoadingOperation() => LoadingCounter++;
	public void RemoveLoadingOperation() => LoadingCounter--;
}