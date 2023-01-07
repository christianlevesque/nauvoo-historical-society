using System;

namespace Sienar.Errors;

public class SienarUnauthorizedException : ApplicationException
{

	public SienarUnauthorizedException(string message = "You are not authorized to perform that action") : base(message)
	{
	}
}