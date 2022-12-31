namespace Server.Tools.Constants;

public static class Urls
{
	public const string Home = "/";
	public const string Privacy = "/Privacy";

	public static class Account
	{
		private const string AccountPrefix = "/account";
		public const string Login = $"{AccountPrefix}/login";
		public const string Logout = $"{AccountPrefix}/logout";
		public const string Deleted = $"{AccountPrefix}/deleted";
		public const string Forbidden = $"{AccountPrefix}/forbidden";

		public static class Register
		{
			private const string RegisterPrefix = $"{AccountPrefix}/register";
			public const string RegisterIndex = RegisterPrefix;
			public const string Successful = $"{RegisterPrefix}/successful";
		}

		public static class Confirm
		{
			private const string ConfirmPrefix = $"{AccountPrefix}/confirm";
			public const string ConfirmIndex = ConfirmPrefix;
			public const string Successful = $"{ConfirmPrefix}/successful";
		}

		public static class ForgotPassword
		{
			private const string ForgotPasswordPrefix = $"{AccountPrefix}/forgot-password";
			public const string ForgotPasswordIndex = ForgotPasswordPrefix;
			public const string Successful = $"{ForgotPasswordPrefix}/successful";
		}

		public static class ResetPassword
		{
			private const string ResetPasswordPrefix = $"{AccountPrefix}/reset-password";
			public const string ResetPasswordIndex = ResetPasswordPrefix;
			public const string Successful = $"{ResetPasswordPrefix}/successful";
		}
	}

	public static class Dashboard
	{
		private const string DashboardPrefix = "/dashboard";
		public const string DashboardIndex = DashboardPrefix;

		public static class MyAccount
		{
			private const string MyAccountPrefix = $"{DashboardPrefix}/my-account";
			public const string PersonalData = $"{MyAccountPrefix}/personal-data";

			public static class EmailChange
			{
				private const string EmailChangePrefix = $"{MyAccountPrefix}/email";
				public const string EmailChangeIndex = EmailChangePrefix;
				public const string Requested = $"{EmailChangePrefix}/requested";
				public const string Confirm = $"{EmailChangePrefix}/confirm";
				public const string Successful = $"{EmailChangePrefix}/successful";
			}

			public static class PasswordChange
			{
				private const string PasswordChangePrefix = $"{MyAccountPrefix}/password";
				public const string PasswordChangeIndex = PasswordChangePrefix;
				public const string Successful = $"{PasswordChangePrefix}/successful";
			}
		}

		public static class Users
		{
			private const string Prefix = $"{DashboardPrefix}/users";
			public const string UsersIndex = Prefix;
		}

		public static class States
		{
			private const string Prefix = $"{Dashboard.DashboardPrefix}/states";
			public const string StatesIndex = Prefix;
			public const string Add = $"{Prefix}/add";
			public const string Edit = $"{Prefix}/edit";
		}
	}
}