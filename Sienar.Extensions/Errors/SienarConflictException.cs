using System;

namespace Sienar.Errors;

public class SienarConflictException : ApplicationException
{
	public SienarConflictException(string message = "There were conflicting records in the database") : base(message)
	{
	}
}