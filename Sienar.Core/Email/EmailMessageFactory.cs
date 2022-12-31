using Autoinjector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Sienar.Email;

[Service(ServiceLifetime.Scoped, typeof(IEmailMessageFactory))]
public class EmailMessageFactory : IEmailMessageFactory
{
	private readonly string _teamName;
	private readonly string _confirmationUrl;
	private readonly string _emailChangeUrl;
	private readonly string _resetPasswordUrl;

	public EmailMessageFactory(IOptions<EmailOptions> options)
	{
		var url = options.Value.ApplicationUrl;
		_teamName = options.Value.EmailSignature;
		_confirmationUrl = $"{url}/account/confirm";
		_emailChangeUrl = $"{url}/dashboard/my-account/email/confirm";
		_resetPasswordUrl = $"{url}/account/reset-password";
	}

	public string WelcomeEmailHtml(string username, string userId, string verificationCode)
	{
		return $"<!DOCTYPE html><html><head><title>Welcome,{username}!</title></head><body><p>Hello {username},</p><p>Thank you for registering. Before you can sign in, you need to confirm your account by clicking the following link: <a href='{GenerateEmailConfirmationLink(userId, verificationCode)}'>confirm account</a></p><p>Regards,</p><p>{_teamName}</p></body></html>";
	}

	public string WelcomeEmailText(string username, string userId, string verificationCode)
	{
		return $"Hello {username},\n\n" +
		       $"Thank you for registering. Before you can sign in, you need to confirm your account by copying the following link into your browser: {GenerateEmailConfirmationLink(userId, verificationCode)}\n\n" +
		       "Regards,\n\n" +
		       _teamName;
	}

	private string GenerateEmailConfirmationLink(string userId, string verificationCode)
	{
		return $"{_confirmationUrl}?userId={userId}&code={verificationCode}";
	}

	public string ChangeEmailHtml(string username, string newEmail, string userId, string verificationCode)
	{
		return $"<!DOCTYPE html><html><head><title>Confirm your new email address, {username}!</title></head><body><p>Hello {username},</p><p>Before you can sign in using your new email address, you need to confirm it by clicking the following link: <a href='{GenerateChangeEmailConfirmationLink(userId, newEmail, verificationCode)}'>confirm account</a></p><p>Regards,</p><p>{_teamName}</p></body></html>";
	}

	public string ChangeEmailText(string username, string newEmail, string userId, string verificationCode)
	{
		return $"Hello {username},\n\n" +
		       $"Before you can sign in using your new email address, you need to confirm it by copying the following link into your browser: {GenerateChangeEmailConfirmationLink(userId, newEmail, verificationCode)}\n\n" +
		       "Regards,\n\n" +
		       _teamName;
	}

	private string GenerateChangeEmailConfirmationLink(string userId, string newEmail, string verificationCode)
	{
		return $"{_emailChangeUrl}?userId={userId}&email={newEmail}&code={verificationCode}";
	}

	public string ResetPasswordHtml(string username, string userId, string verificationCode)
	{
		return $"<!DOCTYPE html><html><head><title>Password Reset Request</title></head><body><p>Hello {username},</p><p>We received a request to reset your password. If this was you, you can reset your password by clicking the following link: <a href='{GenerateResetPasswordLink(userId, verificationCode)}'>reset password</a></p><p>If this was not you, delete this email. The reset code will expire in 30 minutes and your account details will not be changed.</p><p>Regards,</p><p>{_teamName}</p></body></html>";
	}

	public string ResetPasswordText(string username, string userId, string verificationCode)
	{
		return $"Hello {username},\n\n" +
		       $"We received a request to reset your password. If this was you, you can reset your password by copying the following link into your browser: {GenerateResetPasswordLink(userId, verificationCode)}\n\n" +
		       "If this was not you, delete this email. The reset code will expire in 30 minutes and your account details will not be changed.\n\n" +
		       "Regards,\n\n" +
		       _teamName;
	}

	private string GenerateResetPasswordLink(string userId, string verificationCode)
	{
		return $"{_resetPasswordUrl}?userId={userId}&code={verificationCode}";
	}
}

public interface IEmailMessageFactory
{
	string WelcomeEmailHtml(string username, string userId, string verificationCode);
	string WelcomeEmailText(string username, string userId, string verificationCode);
	string ChangeEmailHtml(string username, string newEmail, string userId, string verificationCode);
	string ChangeEmailText(string username, string newEmail, string userId, string verificationCode);
	string ResetPasswordHtml(string username, string userId, string verificationCode);
	string ResetPasswordText(string username, string userId, string verificationCode);
}
