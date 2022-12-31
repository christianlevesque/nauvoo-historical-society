using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Server.Pages;

[AllowAnonymous]
public class Index : PageModel
{
	public void OnGet()
	{
		
	}
}