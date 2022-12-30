namespace Server.Tools;

public static class Urls
{
	public const string Home = "/";

	public static class Account
	{
		private const string Prefix = "/account";
		public const string Login = $"{Prefix}/login";
		public const string Logout = $"{Prefix}/logout";
		public const string Deleted = $"{Prefix}/deleted";
		public const string Forbidden = $"{Prefix}/forbidden";

		public static class Register
		{
			private const string RegisterPrefix = $"{Prefix}/register";
			public const string Index = RegisterPrefix;
			public const string Successful = $"{RegisterPrefix}/successful";
		}

		public static class Confirm
		{
			private const string Prefix = $"{Account.Prefix}/confirm";
			public const string Index = Prefix;
			public const string Successful = $"{Prefix}/successful";
		}

		public static class ForgotPassword
		{
			private const string Prefix = $"{Account.Prefix}/forgot-password";
			public const string Index = Prefix;
			public const string Successful = $"{Prefix}/successful";
		}

		public static class ResetPassword
		{
			private const string Prefix = $"{Account.Prefix}/reset-password";
			public const string Index = Prefix;
			public const string Successful = $"{Prefix}/successful";
		}
	}

	public static class Dashboard
	{
		private const string Prefix = "/dashboard";
		public const string Index = Prefix;

		public static class MyAccount
		{
			private const string Prefix = $"{Dashboard.Prefix}/my-account";
			public const string PersonalData = $"{Prefix}/personal-data";

			public static class EmailChange
			{
				private const string Prefix = $"{MyAccount.Prefix}/email";
				public const string Index = Prefix;
				public const string Requested = $"{Prefix}/requested";
				public const string Confirm = $"{Prefix}/confirm";
				public const string Successful = $"{Prefix}/successful";
			}

			public static class PasswordChange
			{
				private const string Prefix = $"{MyAccount.Prefix}/password";
				public const string Index = Prefix;
				public const string Successful = $"{Prefix}/successful";
			}
		}

		public static class Users
		{
			private const string Prefix = $"{Dashboard.Prefix}/users";
			public const string Index = Prefix;
		}

		public static class States
		{
			private const string Prefix = $"{Dashboard.Prefix}/states";
			public const string Index = Prefix;
			public const string Add = $"{Prefix}/add";
			public const string Edit = $"{Prefix}/edit";
		}
	}
}