using System;

namespace Sienar.Errors;

public class SienarDbException : ApplicationException
{
	private const string DefaultMessage = "An unknown database error has occurred";
	public SienarDbException(string message = DefaultMessage) : base(message)
	{
	}

	public SienarDbException(Exception innerException) : base(DefaultMessage, innerException)
	{
	}

	/// <inheritdoc />
	public SienarDbException(string message, Exception innerException) : base(message, innerException)
	{
	}
}