using MudBlazor;

namespace Client.Shared.UI.Forms;

public class Honeypot : MudTextField<string>
{
	/// <inheritdoc />
	public Honeypot() : base()
	{
		Class = "extra-special-form-field";
		Label = "Please enter your secret key";
		Immediate = true;
		UserAttributes.Add("autocomplete", "off");
		UserAttributes.Add("tabindex", -1);
	}
}