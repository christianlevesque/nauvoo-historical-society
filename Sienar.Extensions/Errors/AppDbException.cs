using System;

namespace Sienar.Errors;

public class AppDbException : ApplicationException
{
	private const string DefaultMessage = "An unknown database error has occurred";
	public AppDbException(string message = DefaultMessage) : base(message)
	{
	}

	public AppDbException(Exception innerException) : base(DefaultMessage, innerException)
	{
	}

	/// <inheritdoc />
	public AppDbException(string message, Exception innerException) : base(message, innerException)
	{
	}
}