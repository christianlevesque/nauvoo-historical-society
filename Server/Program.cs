using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Server.Policies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Server.Tools;
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
		.AddServerServices(builder.Environment, builder.Configuration);

	var app = builder.Build();

	if (app.Environment.IsDevelopment())
	{
		app.UseDeveloperExceptionPage();
		app.UseWebAssemblyDebugging();
	}

	app.UseStaticFiles();

	app.UseSerilogRequestLogging();
	app.UseRouting();

	if (app.Environment.IsDevelopment())
	{
		app.UseCors();
	}

	app.UseAuthentication();
	app.UseAuthorization();
	app.MapRazorPages();

	app.MapBlazorHub();
	app.MapFallbackToPage("/_Host");

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
	var schemes = new List<string>
	{
		JwtBearerDefaults.AuthenticationScheme
	};

	// Configure Authorization pipelines
	services.AddAuthorization(options =>
	{
		var defaultPolicy = new AuthorizationPolicyBuilder
		{
			AuthenticationSchemes = schemes
		};

		options.DefaultPolicy = defaultPolicy
			.RequireAuthenticatedUser()
		    .Build();

		options.AddPolicy(
			NotLoggedInRequirement.Name,
			policy =>
			{
				policy.AuthenticationSchemes = schemes;
				policy.Requirements.Add(new NotLoggedInRequirement());
			}
		);
	});

	services.AddTransient<IAuthorizationHandler, NotLoggedInHandler>();
}
