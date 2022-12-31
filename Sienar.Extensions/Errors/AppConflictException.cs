using System;

namespace Sienar.Errors;

public class AppConflictException : ApplicationException
{
	public AppConflictException(string message = "There were conflicting records in the database") : base(message)
	{
	}
}