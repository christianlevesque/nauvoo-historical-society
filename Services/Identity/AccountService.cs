using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Account;
using Autoinjector;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Repositories.Identity;
using Services.Email;
using Services.Errors;

namespace Services.Identity;

[Service(ServiceLifetime.Scoped, typeof(IAccountService))]
public class AccountService : IAccountService
{
	private readonly SignInService _signInService;
	private readonly UserService _userService;
	private readonly IDtoAdapter<ApplicationUser, ApplicationUserDto> _userDtoAdapter;
	private readonly IEmailSenderService _emailService;
	private readonly ILogger<IAccountService> _logger;

	public AccountService(SignInService signInService,
	                      UserService userService,
	                      IDtoAdapter<ApplicationUser, ApplicationUserDto> userDtoAdapter,
	                      IEmailSenderService emailService,
	                      ILogger<IAccountService> logger)
	{
		_signInService = signInService;
		_userService = userService;
		_userDtoAdapter = userDtoAdapter;
		_emailService = emailService;
		_logger = logger;
	}

	/// <inheritdoc/>
	public async Task<ServiceResult> Register(RegisterDto userData)
	{
		// Log honeypot time-to-complete for analytics purposes
		_logger.LogInformation("Form submission completed in {time} seconds", userData.TimeToComplete.TotalSeconds);

		// Check the honeypot and immediately return if it fails
		if (userData.IsSpambot)
		{
			_logger.LogError("Spambot detected! Value passed to honeypot was '{value}'", userData.SecretKeyField);
			return ServiceResult.Ok();
		}

		// Verify they accepted the terms and conditions
		if (!userData.AcceptTos)
		{
			return ServiceResult.Unprocessable(ErrorMessages.Account.MustAcceptTos);
		}

		// Check for duplicates
		var existingUser = await _userService.FindByNameAsync(userData.UserName);
		if (existingUser != null)
		{
			return ServiceResult.Unprocessable(ErrorMessages.Account.UsernameTaken);
		}

		existingUser = await _userService.FindByEmailAsync(userData.Email);
		if (existingUser != null)
		{
			return ServiceResult.Unprocessable(ErrorMessages.Account.EmailTaken);
		}

		// Checks passed. Make a new user
		var user = new ApplicationUser
		{
			UserName = userData.UserName,
			Email = userData.Email
		};

		// Try to create that user with the given password
		var result = await _userService.CreateAsync(user, userData.Password);
		if (!result.Succeeded)
		{
			return ServiceResult.Unprocessable(result.Errors.First().Description);
		}

		await SendWelcomeEmail(user);

		return ServiceResult.Ok();
	}

	/// <inheritdoc/>
	public async Task<ServiceResult> Login(LoginDto login)
	{
		// Log honeypot time-to-complete for analytics purposes
		_logger.LogInformation("Form submission completed in {time} seconds", login.TimeToComplete.TotalSeconds);

		// Check the honeypot and immediately return if it fails
		if (login.IsSpambot)
		{
			_logger.LogError("Spambot detected! Value passed to honeypot was '{value}'", login.SecretKeyField);
			return ServiceResult.Ok();
		}

		// Check if the account name exists
		// If not, check if the user used their email address
		var user = await _userService.FindByNameAsync(login.AccountName);
		user ??= await _userService.FindByEmailAsync(login.AccountName);

		// If still not, return unauthorized
		if (user == null)
		{
			return ServiceResult.NotFound(ErrorMessages.Account.LoginFailedNotFound);
		}

		// If user is locked out, tell them when the lockout date ends
		if (await _userService.IsLockedOutAsync(user))
		{
			return ServiceResult.Unauthorized(ErrorMessages.Account.GetLockoutMessage(user.LockoutEnd));
		}

		login.AccountName = user.UserName;

		// If a user exists but isn't confirmed
		// check their password. If it matches,
		// resend their verification email
		if (!await _userService.CheckPasswordAsync(user, login.Password))
		{
			return ServiceResult.Unauthorized(ErrorMessages.Account.LoginFailedInvalid);
		}

		if (!user.EmailConfirmed)
		{
			await SendWelcomeEmail(user);
			return ServiceResult.Unauthorized(ErrorMessages.Account.LoginFailedNotConfirmed);
		}

		var token = await _signInService.SignInApiUser(user);

		return ServiceResult.Ok(new TokenDto(token));
	}

	/// <inheritdoc/>
	public async Task<ServiceResult> ConfirmAccount(ConfirmAccountDto account)
	{
		var user = await _userService.FindByIdAsync(account.UserId);
		if (user == null)
		{
			return ServiceResult.NotFound(ErrorMessages.Account.AccountErrorInvalidId);
		}

		account.VerificationCode = Base64UrlEncoder.Decode(account.VerificationCode);

		var result = await _userService.ConfirmEmailAsync(user, account.VerificationCode);

		return result.Succeeded
			       ? ServiceResult.Ok()
			       : ServiceResult.Unauthorized(result.Errors.First().Description);
	}

	/// <inheritdoc/>
	public async Task<ServiceResult> InitiateEmailChange(InitiateEmailChangeDto emailChange, ClaimsPrincipal userClaims)
	{
		var user = await _userService.GetUserAsync(userClaims);
		if (!await _userService.CheckPasswordAsync(user, emailChange.ConfirmPassword))
		{
			return ServiceResult.Unauthorized(ErrorMessages.Account.LoginFailedInvalid);
		}

		user.PendingEmail = emailChange.Email;
		await _userService.UpdateAsync(user);

		await SendEmailChangeConfirmationEmail(user);

		return ServiceResult.Ok();
	}

	/// <inheritdoc/>
	public async Task<ServiceResult> PerformEmailChange(PerformEmailChangeDto emailChange, ClaimsPrincipal userClaims)
	{
		var user = await _userService.GetUserAsync(userClaims);
		if (user.PendingEmail != emailChange.NewEmail)
		{
			return ServiceResult.Conflict(ErrorMessages.Account.EmailErrorWrongEmail);
		}

		if (user.Id != emailChange.UserId)
		{
			return ServiceResult.Conflict(ErrorMessages.Account.AccountErrorWrongId);
		}

		emailChange.VerificationCode = Base64UrlEncoder.Decode(emailChange.VerificationCode);

		var result = await _userService.ChangeEmailAsync(user, user.PendingEmail, emailChange.VerificationCode);

		return result.Succeeded
			       ? ServiceResult.Ok()
			       : ServiceResult.Unprocessable(result.Errors.First().Description);
	}

	/// <inheritdoc/>
	public async Task<ServiceResult> ChangePassword(ChangePasswordDto passwordChange, ClaimsPrincipal userClaims)
	{
		var user = await _userService.GetUserAsync(userClaims);
		var result = await _userService.ChangePasswordAsync(user, passwordChange.CurrentPassword, passwordChange.NewPassword);
		return result.Succeeded
			       ? ServiceResult.Ok()
			       : ServiceResult.Unauthorized(result.Errors.First().Description);
	}

	/// <inheritdoc/>
	public async Task<ServiceResult> ForgotPassword(ForgotPasswordDto passwordRequest)
	{
		// Log honeypot time-to-complete for analytics purposes
		_logger.LogInformation("Form submission completed in {time} seconds", passwordRequest.TimeToComplete.TotalSeconds);

		// Check the honeypot and immediately return if it fails
		if (passwordRequest.IsSpambot)
		{
			_logger.LogError("Spambot detected! Value passed to honeypot was '{value}'", passwordRequest.SecretKeyField);
			return ServiceResult.Ok();
		}

		var user = await _userService.FindByNameAsync(passwordRequest.AccountName);
		user ??= await _userService.FindByEmailAsync(passwordRequest.AccountName);

		// They don't need to know whether the user exists or not
		// so if the user isn't null, send the email
		// otherwise, just return Ok anyway
		if (user != null)
		{
			await SendPasswordResetEmail(user);
		}

		return ServiceResult.Ok();
	}

	/// <inheritdoc/>
	public async Task<ServiceResult> ResetPassword(ResetPasswordDto resetPassword)
	{
		// Log honeypot time-to-complete for analytics purposes
		_logger.LogInformation("Form submission completed in {time} seconds", resetPassword.TimeToComplete.TotalSeconds);

		// Check the honeypot and immediately return if it fails
		if (resetPassword.IsSpambot)
		{
			_logger.LogError("Spambot detected! Value passed to honeypot was '{value}'", resetPassword.SecretKeyField);
			return ServiceResult.Ok();
		}

		var user = await _userService.FindByIdAsync(resetPassword.UserId);
		if (user == null)
		{
			return ServiceResult.NotFound(ErrorMessages.Account.AccountErrorInvalidId);
		}

		resetPassword.VerificationCode = Base64UrlEncoder.Decode(resetPassword.VerificationCode);

		var result = await _userService.ResetPasswordAsync(user, resetPassword.VerificationCode, resetPassword.NewPassword);
		return result.Succeeded
			       ? ServiceResult.Ok()
			       : ServiceResult.Unprocessable(result.Errors.First().Description);
	}

	/// <inheritdoc/>
	public async Task<ServiceResult> GetPersonalData(ClaimsPrincipal userClaims)
	{
		var user = await _userService.GetUserAsync(userClaims);

		// Only include personal data for download
		var personalData = new Dictionary<string, string>();
		var personalDataProps = user.GetType()
		                            .GetProperties()
		                            .Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
		foreach (var p in personalDataProps)
		{
			personalData.Add(p.Name, p.GetValue(user)
			                          ?.ToString() ?? "null");
		}

		var logins = await _userService.GetLoginsAsync(user);
		foreach (var l in logins)
		{
			personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
		}

		var authKey = await _userService.GetAuthenticatorKeyAsync(user);
		if (!string.IsNullOrEmpty(authKey))
		{
			personalData.Add("Authenticator Key", authKey);
		}

		return ServiceResult.Ok(JsonSerializer.SerializeToUtf8Bytes(personalData));
	}

	/// <inheritdoc/>
	public async Task<ServiceResult> DeleteAccount(ClaimsPrincipal userClaims)
	{
		var user = await _userService.GetUserAsync(userClaims);
		var result = await _userService.DeleteAsync(user);
		return result.Succeeded
			       ? ServiceResult.Ok()
			       : ServiceResult.Unprocessable(result.Errors.First().Description);
	}

	/// <inheritdoc/>
	public async Task<ServiceResult> GetUserInfo(ClaimsPrincipal userClaims)
	{
		var user = await _userService.GetUserAsync(userClaims);
		return user == null
			       ? ServiceResult.Unauthorized(ErrorMessages.Account.LoginRequired)
			       : ServiceResult.Ok(await _userDtoAdapter.MapToDto(user));
	}

	private async Task SendWelcomeEmail(ApplicationUser user)
	{
		var code = await _userService.GenerateEmailConfirmationTokenAsync(user);
		await _emailService.SendWelcomeEmail(user.UserName, user.Email, user.Id, Base64UrlEncoder.Encode(code));
	}

	private async Task SendEmailChangeConfirmationEmail(ApplicationUser user)
	{
		var code = await _userService.GenerateChangeEmailTokenAsync(user, user.PendingEmail);
		await _emailService.SendEmailChangeConfirmationEmail(user.UserName, user.PendingEmail, user.Id, Base64UrlEncoder.Encode(code));
	}

	private async Task SendPasswordResetEmail(ApplicationUser user)
	{
		var code = await _userService.GeneratePasswordResetTokenAsync(user);
		await _emailService.SendPasswordResetEmail(user.UserName, user.Email, user.Id, Base64UrlEncoder.Encode(code));
	}
}

public interface IAccountService
{
	/// <summary>
	/// Registers a new user in the database
	/// </summary>
	/// <param name="userData">The data to create the user with</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult> Register(RegisterDto userData);

	/// <summary>
	/// Logs a user into the application
	/// </summary>
	/// <param name="login">The user credentials to log in with</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult> Login(LoginDto login);

	/// <summary>
	/// Confirms a new user's account via a unique registration code
	/// </summary>
	/// <param name="account">The information for the user account to confirm</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult> ConfirmAccount(ConfirmAccountDto account);

	/// <summary>
	/// Starts the email change process for the currently logged in user account
	/// </summary>
	/// <param name="emailChange">Verification data from the user initiating the change</param>
	/// <param name="userClaims">The <see cref="ClaimsPrincipal"/> of the currently logged in user</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult> InitiateEmailChange(InitiateEmailChangeDto emailChange, ClaimsPrincipal userClaims);

	/// <summary>
	/// Performs an email change for the currently logged in user account
	/// </summary>
	/// <param name="emailChange">The information for the user account whose email to change</param>
	/// <param name="userClaims">The <see cref="ClaimsPrincipal"/> of the currently logged in user</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult> PerformEmailChange(PerformEmailChangeDto emailChange, ClaimsPrincipal userClaims);

	/// <summary>
	/// Changes the password for the currently logged in user account
	/// </summary>
	/// <param name="passwordChange">Information verifying the password change request</param>
	/// <param name="userClaims">The <see cref="ClaimsPrincipal"/> of the currently logged in user</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult> ChangePassword(ChangePasswordDto passwordChange, ClaimsPrincipal userClaims);

	/// <summary>
	/// Initiates the password reset process for the specified user account
	/// </summary>
	/// <param name="passwordRequest">Information to determine which user account to start the password reset process for</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult> ForgotPassword(ForgotPasswordDto passwordRequest);

	/// <summary>
	/// Performs the actual password reset for the specified user account
	/// </summary>
	/// <param name="resetPassword">Verification data and new password for the user performing the password reset</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult> ResetPassword(ResetPasswordDto resetPassword);

	/// <summary>
	/// Returns the personal data of the currently logged in user, in binary format
	/// </summary>
	/// <param name="userClaims">The <see cref="ClaimsPrincipal"/> of the currently logged in user</param>
	/// <returns></returns>
	Task<ServiceResult> GetPersonalData(ClaimsPrincipal userClaims);

	/// <summary>
	/// Deletes the account of the currently logged in user
	/// </summary>
	/// <param name="userClaims">The <see cref="ClaimsPrincipal"/> of the currently logged in user</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult> DeleteAccount(ClaimsPrincipal userClaims);

	/// <summary>
	/// Gets the account information for the currently logged in user
	/// </summary>
	/// <param name="userClaims">The <see cref="ClaimsPrincipal"/> of the currently logged in user</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult> GetUserInfo(ClaimsPrincipal userClaims);
}