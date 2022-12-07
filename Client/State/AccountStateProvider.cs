using System;
using Core.Account;
using Autoinjector;
using Microsoft.Extensions.DependencyInjection;

namespace Client.State;

[Service(ServiceLifetime.Scoped)]
public class AccountStateProvider
{
	private ApplicationUserDto? _user;

	public ApplicationUserDto? User
	{
		get => _user;
		set
		{
			_user = value;
			NotifyStateChanged();
		}
	}

	public event Action? OnChange;

	private void NotifyStateChanged() => OnChange?.Invoke();
}