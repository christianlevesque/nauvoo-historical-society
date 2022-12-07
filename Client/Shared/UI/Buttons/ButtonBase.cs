using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Shared.UI.Buttons;

public class ButtonBase : MudButton
{
	public ButtonBase()
	{
		ButtonType = ButtonType.Button;
		Variant = Variant.Text;
	}
}