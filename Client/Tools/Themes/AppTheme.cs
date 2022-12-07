using MudBlazor;

namespace Client.Tools.Themes;

public class AppTheme : MudTheme
{
	public AppTheme()
	{
		// Palette.AppbarBackground = Palette.White;

		Typography.Button.FontSize = Typography.H6.FontSize;
		Typography.Button.FontWeight = 400;
		Typography.Button.TextTransform = "none";
	}
}