using MudBlazor;

namespace Client.Shared.UI.Helpers;

public class LoadingSpinner : MudProgressCircular
{
	public LoadingSpinner()
	{
		Color = Color.Primary;
		Indeterminate = true;
	}
}