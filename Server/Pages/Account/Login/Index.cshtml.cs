using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Tools;
using Services.Identity;

namespace Server.Pages.Account.Login;

[IgnoreAntiforgeryToken]
public class Index : PageModel
{
	private readonly IAccountService _accountService;

	[BindProperty]
	public LoginDto Login { get; set; } = new();

	public string? ErrorMessage { get; set; }

	public Index(IAccountService accountService)
	{
		_accountService = accountService;
	}

	public void OnGet() {}

	public async Task<IActionResult> OnPost()
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		var result = await _accountService.Login(Login);
		if (!result.WasSuccessful)
		{
			ErrorMessage = result.ErrorMessage;
			return Page();
		}

		return Redirect(Urls.Dashboard.Index);
	}
}