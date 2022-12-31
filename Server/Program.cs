using System;
using System.Collections.Generic;
using System.IO;
using Server.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Server.Tools;
using Server.Tools.Constants;
using Services;

var path = $"{Directory.GetCurrentDirectory()}/log";
if (!Directory.Exists(path))
{
	Directory.CreateDirectory(path);
}

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.CreateBootstrapLogger();

Log.Information("Starting #sitename#"); // TODO: set up site identity

try
{
	var builder = WebApplication.CreateBuilder(args);

	builder.Host.UseSerilog((ctx, config) =>
	{
		config.MinimumLevel.Debug()
			.WriteTo.File(
				$"{path}/log-.log",
				rollingInterval: RollingInterval.Hour,
				restrictedToMinimumLevel: LogEventLevel.Error,
				retainedFileCountLimit: 168)
			.WriteTo.Console()
			.WriteTo.Debug();
	});

	AddAuthentication(builder.Services);
	AddCustomAuthorization(builder.Services);

	builder.Services
		.AddServices(builder.Configuration, builder.Environment)
		.AddServerServices();

	var app = builder.Build();

	if (app.Environment.IsDevelopment())
	{
		app.UseDeveloperExceptionPage();
	}

	app.UseStaticFiles();

	app.UseSerilogRequestLogging();
	app.UseRouting();

	app.UseAuthentication();
	app.UseAuthorization();
	app.MapRazorPages();

	app.Run();
}
catch (Exception e)
{
	Log.Fatal(e, "#sitename# terminated unexpectedly"); // TODO: set up site identity
}
finally
{
	Log.Information("Shutting down #sitename#"); // TODO: set up site identity
	Log.CloseAndFlush();
}

void AddAuthentication(IServiceCollection services)
{
	services.AddAuthentication(o =>
		{
			o.DefaultScheme = IdentityConstants.ApplicationScheme;
		})
		.AddCookie(IdentityConstants.ApplicationScheme, o =>
		{
			o.Cookie.Name = IdentityConstants.ApplicationScheme;
			o.Cookie.SameSite = SameSiteMode.Strict;
			o.ExpireTimeSpan = TimeSpan.FromHours(24);
			o.LoginPath = Urls.Account.Login;
			o.LogoutPath = Urls.Account.Logout;
			o.AccessDeniedPath = Urls.Account.Forbidden;
			o.SlidingExpiration = true;
		});
}

void AddCustomAuthorization(IServiceCollection services)
{
	// Configure Authorization pipelines
	services.AddAuthorization(options =>
	{
		options.AddPolicy(
			NotLoggedInRequirement.Name,
			policy =>
			{
				policy.AuthenticationSchemes = new List<string>
				{
					IdentityConstants.ApplicationScheme
				};
				policy.Requirements.Add(new NotLoggedInRequirement());
			}
		);
	});

	services.AddTransient<IAuthorizationHandler, NotLoggedInHandler>();
}
