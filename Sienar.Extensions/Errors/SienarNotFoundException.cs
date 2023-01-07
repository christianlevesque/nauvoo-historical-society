using System;

namespace Sienar.Errors;

public class SienarNotFoundException : ApplicationException
{
	public SienarNotFoundException(string message = "The requested resource could not be found") : base(message)
	{
	}
}