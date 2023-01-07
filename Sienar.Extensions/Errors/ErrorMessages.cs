using System;
using System.Diagnostics.CodeAnalysis;

namespace Sienar.Errors;

[ExcludeFromCodeCoverage]
public static class ErrorMessages
{
	public const string Database = "There was a database error.";
	public const string ContactIt = "Please contact the IT team if your problem persists.";

	public static class Account
	{
		public const string MustAcceptTos = "You must accept the Terms of Service to create an account.";
		public const string UsernameTaken = "That username is already taken.";
		public const string EmailTaken = "That email address is already taken. Do you need to reset your password instead?";
		public const string LoginRequired = "You must be logged in to perform that action.";
		public const string LoginFailedNotFound = "No account with those credentials was found.";
		public const string LoginFailedInvalid = "Invalid credentials supplied.";
		public const string LoginFailedNotConfirmed = "You have not confirmed your email address. Please check your email for a confirmation link and click it to confirm your email address.";

		public const string EmailErrorWrongEmail = "That email does not match the email change request you initiated.";

		public const string AccountErrorWrongId = "The User ID supplied does not match your account.";
		public const string AccountErrorInvalidId = "No account was found with that User ID.";

		/// <summary>
		/// Returns an error message indicating the lockout status of a user's account
		/// </summary>
		/// <param name="offset">The </param>
		/// <returns></returns>
		public static string GetLockoutMessage(DateTimeOffset? offset)
		{
			return offset == DateTimeOffset.MaxValue
				       ? "Your account is permanently locked."
				       : $"Your account is currently locked. The lock will end on {offset:D} at {offset:h:mm:ss tt}.";
		}
	}

	public static class States
	{
		public const string DuplicateName = "A State with that name already exists.";
		public const string DuplicateAbbreviation = "A State with that abbreviation already exists.";
	}

	public static class Generic
	{
		public const string UnhandledByServiceLayer = "An unknown error occurred that wasn't handled by the service layer.";
		public const string Unknown = "An unknown error has occurred.";
		public const string NotFound = "The requested resource was not found.";
		public const string NotLoggedIn = "You must be logged in to perform that action.";
		public const string NoPermission = "You do not have permission to perform that action.";
		public const string Unprocessable = "Your request could not be processed with the provided data. Please check your data and try again.";
		public const string DataConflict = "The data you entered is not valid. Please check your data and try again.";
		public const string DataConcurrencyConflict = "The database record you requested has been updated since you last accessed it. Please reload the page and try again.";
	}
}