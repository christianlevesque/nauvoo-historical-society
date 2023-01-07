using System;

namespace Sienar.Errors;

public class SienarUnprocessableEntityException : ApplicationException
{
	public SienarUnprocessableEntityException(string message = "The entity was unable to be created or updated with the supplied information") : base(message)
	{
	}
}