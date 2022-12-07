namespace Server.Tools.Constants;

public static class Urls
{
	public const string Index = "/Index";
	public const string Privacy = "/Privacy";

	public static class Account
	{
		public const string Base = "/Account";
		public const string Index = Base;
		public const string Login = $"{Base}/Login";
		public const string Logout = $"{Base}/Logout";
		public const string Forbidden = $"{Base}/Forbidden";
	}
}