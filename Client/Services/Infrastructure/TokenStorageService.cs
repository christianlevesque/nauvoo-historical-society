using System.Threading.Tasks;
using Blazored.LocalStorage;
using Autoinjector;
using Microsoft.Extensions.DependencyInjection;
using Client.Tools;

namespace Client.Services.Infrastructure;

[Service(ServiceLifetime.Scoped, typeof(ITokenStorageService))]
public class TokenStorageService : ITokenStorageService
{
	private readonly ILocalStorageService _storage;

	public TokenStorageService(ILocalStorageService storage)
	{
		_storage = storage;
	}

	public ValueTask<string> GetToken() => _storage.GetItemAsStringAsync(Constants.AuthTokenName);

	public ValueTask SetToken(string token) => _storage.SetItemAsStringAsync(Constants.AuthTokenName, token);

	public ValueTask UnsetToken() => _storage.RemoveItemAsync(Constants.AuthTokenName);
}

public interface ITokenStorageService
{
	public ValueTask<string> GetToken();
	public ValueTask SetToken(string token);
	public ValueTask UnsetToken();
}