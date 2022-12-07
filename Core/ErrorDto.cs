namespace Core;

public class ErrorDto
{
	public string Message { get; }

	public ErrorDto(string message)
	{
		Message = message;
	}
}