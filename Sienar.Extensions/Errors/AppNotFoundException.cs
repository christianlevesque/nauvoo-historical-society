using System;
using System.Runtime.Serialization;

namespace Sienar.Errors;

public class AppNotFoundException : ApplicationException
{
	public AppNotFoundException(string message = "The requested resource could not be found") : base(message)
	{
	}
}