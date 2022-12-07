using System;
using System.Runtime.Serialization;

namespace Services.Errors;

public class AppUnauthorizedException : ApplicationException
{

	public AppUnauthorizedException(string message = "You are not authorized to perform that action") : base(message)
	{
	}
}