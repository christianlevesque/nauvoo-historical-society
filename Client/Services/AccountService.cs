using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Account;
using Autoinjector;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Client.Services.Infrastructure;
using Client.State;

namespace Client.Services;

[Service(ServiceLifetime.Scoped, typeof(IAccountService))]
public class AccountService : BaseHttpService<AccountService>, IAccountService
{
	private readonly AuthStateProvider _authState;
	private readonly AccountStateProvider _accountState;

	public AccountService(HttpClient client, ILogger<AccountService> logger, ISnackbarService snackbar, AuthenticationStateProvider authState, AccountStateProvider accountState) : base("account", client, logger, snackbar)
	{
		_authState = (authState as AuthStateProvider)!;
		_accountState = accountState;
	}

	public async Task<bool> Login(LoginDto login)
	{
		Logger.LogInformation("Initializing login process");
		try
		{
			var result = await SendWithRawResponse($"{Endpoint}/login", input: login, method: HttpMethod.Post);
			if (!result.IsSuccessStatusCode)
			{
				await HandleFailureResponse(result);
				return false;
			}

			var response = await GetResponse<TokenDto>(result);

			if (!string.IsNullOrWhiteSpace(response?.Token))
			{
				await _authState.NotifyUserAuthentication(response.Token);

				// This ensures the HttpClient has had the bearer token applied
				await _authState.GetAuthenticationStateAsync();

				// Now we can load the user info
				await LoadUserInfo();
				Logger.LogInformation("User logged in successfully");
				return true;
			}
		}
		catch (Exception e)
		{
			Logger.LogError(e, "Failed to log in user");
			Snackbar.Error(e.Message);
			return false;
		}

		Snackbar.Error(GenericErrorMessage);
		return false;
	}

	public Task<bool> Register(RegisterDto account) => SendRequest(Endpoint, "Registered successfully!", input: account, method: HttpMethod.Post);

	public Task<bool> ConfirmAccount(ConfirmAccountDto account) => SendRequest($"{Endpoint}/confirm", "Account confirmed successfully!", input: account, method: HttpMethod.Post);

	public Task<bool> ChangePassword(ChangePasswordDto account) => SendRequest($"{Endpoint}/password", "Password changed successfully!", input: account, method: HttpMethod.Post);

	public Task<bool> ForgotPassword(ForgotPasswordDto account) => SendRequest($"{Endpoint}/password", "Password reset requested successfully!", input: account, method: HttpMethod.Delete);

	public Task<bool> ResetPassword(ResetPasswordDto account) => SendRequest($"{Endpoint}/password", "Password reset successfully!", input: account, method: HttpMethod.Patch);

	public Task<bool> InitiateEmailChange(InitiateEmailChangeDto account) => SendRequest($"{Endpoint}/email", "Email change requested successfully!", input: account, method: HttpMethod.Post);

	public Task<bool> PerformEmailChange(PerformEmailChangeDto account) => SendRequest($"{Endpoint}/email", "Email changed successfully!", input: account, method: HttpMethod.Patch);

	public async Task<bool> DeleteAccount()
	{
		var deleted = await SendRequest(Endpoint, "Account deleted successfully!", method: HttpMethod.Delete);
		if (deleted)
		{
			await _authState.NotifyUserLogout();
		}

		return deleted;
	}

	public async Task<Stream> GetPersonalData()
	{
		var result = await SendWithRawResponse($"{Endpoint}/data");
		return await result.Content.ReadAsStreamAsync();
	}

	public async Task LoadUserInfo()
	{
		Logger.LogInformation("Getting info for the currently logged in user");
		try
		{
			var result = await SendWithRawResponse($"{Endpoint}/login");

			// If the result is unauthorized
			// we might have a JWT in local storage
			// so clear that out since it's invalid
			if (result.StatusCode == HttpStatusCode.Unauthorized)
			{
				await _authState.NotifyUserLogout();
				return;
			}

			// If the result wasn't successful
			// we don't have a user to store state for
			if (!result.IsSuccessStatusCode)
			{
				return;
			}

			var user = await GetResponse<ApplicationUserDto>(result);
			if (user is null)
			{
				return;
			}

			_accountState.User = user;
		}
		catch (Exception e)
		{
			Logger.LogError(e, "Failed to fetch user info");
		}
	}

	public async Task<bool> Logout()
	{
		try
		{
			await _authState.NotifyUserLogout();
		}
		catch (Exception e)
		{
			Logger.LogError("Failed to log out user: {}", e.Message);
			return false;
		}

		return true;
	}
}

public interface IAccountService
{
	public Task<bool> Login(LoginDto account);
	public Task<bool> Register(RegisterDto account);
	public Task<bool> ConfirmAccount(ConfirmAccountDto account);
	public Task<bool> ForgotPassword(ForgotPasswordDto account);
	public Task<bool> ResetPassword(ResetPasswordDto account);
	public Task<bool> InitiateEmailChange(InitiateEmailChangeDto account);
	public Task<bool> PerformEmailChange(PerformEmailChangeDto account);
	public Task<bool> ChangePassword(ChangePasswordDto account);
	public Task<bool> DeleteAccount();
	public Task<Stream> GetPersonalData();
	public Task LoadUserInfo();
	public Task<bool> Logout();
}