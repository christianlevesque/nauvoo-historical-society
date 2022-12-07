using System.Threading.Tasks;
using Autoinjector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Services.Email;

[Service(ServiceLifetime.Scoped, typeof(IEmailSenderService))]
public class EmailSenderService : IEmailSenderService
{
	public const string SendWelcomeEmailSubject = "Welcome to <Service Name>";
	public const string SendEmailChangeConfirmationEmailSubject = "Confirm Your New Email";
	public const string SendPasswordResetEmailSubject = "Password Reset Confirmation";

	private readonly ILogger<IEmailSenderService> _logger;
	private readonly IOptions<EmailOptions> _options;
	private readonly ISendGridClient _client;
	private readonly IEmailMessageFactory _factory;
	private readonly EmailAddress _from;

	public EmailSenderService(ILogger<IEmailSenderService> logger, IOptions<EmailOptions> options, ISendGridClient client, IEmailMessageFactory factory)
	{
		_logger = logger;
		_options = options;
		_client = client;
		_factory = factory;
		_from = new EmailAddress(_options.Value.EmailFromAddress, _options.Value.EmailFromName);

		_logger.LogInformation("Created {serviceName} instance", typeof(IEmailSenderService));
	}

	public async Task SendWelcomeEmail(string username, string email, string userId, string verificationCode)
	{
		var message = new SendGridMessage
		{
			From = new EmailAddress(_options.Value.EmailFromAddress, _options.Value.EmailFromName),
			Subject = SendWelcomeEmailSubject,
			PlainTextContent = _factory.WelcomeEmailText(username, userId, verificationCode),
			HtmlContent = _factory.WelcomeEmailHtml(username, userId, verificationCode)
		};
		message.AddTo(email, username);

		await SendEmail(message);
		_logger.LogInformation("Sent welcome email");
	}

	public async Task SendEmailChangeConfirmationEmail(string username, string email, string userId, string verificationCode)
	{
		var message = new SendGridMessage
		{
			From = new EmailAddress(_options.Value.EmailFromAddress, _options.Value.EmailFromName),
			Subject = SendEmailChangeConfirmationEmailSubject,
			PlainTextContent = _factory.ChangeEmailText(
				username,
				email,
				userId,
				verificationCode
			),
			HtmlContent = _factory.ChangeEmailHtml(
				username,
				email,
				userId,
				verificationCode
			)
		};
		message.AddTo(email, username);

		await SendEmail(message);
	}

	public async Task SendPasswordResetEmail(string username, string email, string userId, string verificationCode)
	{
		var message = new SendGridMessage
		{
			Subject = SendPasswordResetEmailSubject,
			PlainTextContent = _factory.ResetPasswordText(username, userId, verificationCode),
			HtmlContent = _factory.ResetPasswordHtml(username, userId, verificationCode)
		};
		message.AddTo(email, username);

		await SendEmail(message);
	}

	private async Task SendEmail(SendGridMessage email)
	{
		email.From ??= _from;

		var response = await _client.SendEmailAsync(email);

		_logger.LogInformation("Email sent: {subject}, {status}, {response}",
		                       email.Subject,
		                       response.StatusCode,
		                       await response.Body.ReadAsStringAsync());

		if ((int) response.StatusCode > 299)
		{
			_logger.LogError("Email failed to send: {status}, {response}",
			                 response.StatusCode,
			                 await response.Body.ReadAsStringAsync());
		}
	}
}

public interface IEmailSenderService
{
	Task SendWelcomeEmail(string username, string email, string userId, string verificationCode);
	Task SendEmailChangeConfirmationEmail(string username, string email, string userId, string verificationCode);
	Task SendPasswordResetEmail(string username, string email, string userId, string verificationCode);
}