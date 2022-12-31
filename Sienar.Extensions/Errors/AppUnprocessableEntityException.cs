using System;

namespace Sienar.Errors;

public class AppUnprocessableEntityException : ApplicationException
{
	public AppUnprocessableEntityException(string message = "The entity was unable to be created or updated with the supplied information") : base(message)
	{
	}
}