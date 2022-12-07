using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class Error : PageModel
{
	public string? RequestId { get; set; }
	public string? ErrorMessage { get; private set; }
	public int? ErrorCode { get; private set; }
	public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

	public IActionResult OnGet(int? errorCode)
	{
		ErrorCode = errorCode;
		RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
		ErrorMessage = GetErrorMessage(errorCode);
		return Page();
	}

	private string GetErrorMessage(int? code)
	{
		var exception = HttpContext.Features
			.Get<IExceptionHandlerPathFeature>()
			?.Error;

		return code switch
		{
			StatusCodes.Status401Unauthorized => "You must be logged in to perform that action",
			StatusCodes.Status403Forbidden => "You don't have permission to perform that action",
			_ => "An unknown error has occurred."
		};
	}
}