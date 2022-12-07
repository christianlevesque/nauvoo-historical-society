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

	AddApiAuthentication(builder.Services, builder.Configuration); // TODO: remove or disable if using SSR
	AddStandardAuthentication(builder.Services); // TODO: remove or disable if using Blazor client
	AddCustomAuthorization(builder.Services);

	builder.Services
		.AddServerServices(builder.Environment)
		.AddServices(builder.Configuration, builder.Environment);

	var app = builder.Build();

	// TODO: remove or disable if using Blazor client
	app.UseExceptionHandler("/Error");

	app.UseSerilogRequestLogging();
	app.UseRouting();

	app.UseStaticFiles();

	if (app.Environment.IsDevelopment())
	{
		app.UseCors();
	}

	app.UseAuthentication();
	app.UseAuthorization();
	app.UseEndpoints(e =>
	{
		e.MapControllers();
		e.MapRazorPages(); // TODO: remove or disable if using Blazor client
	});

	// TODO: remove or disable if using Blazor client
	app.UseStatusCodePagesWithReExecute("/Error/{0}");

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

// TODO: remove or disable if using SSR
void AddApiAuthentication(IServiceCollection services, IConfiguration config)
{
	var jwt = config.GetSection("Jwt");

	// Set up Authentication
	services.AddAuthentication(o =>
	        {
		        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	        })
	        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,o =>
	        {
		        var keyBytes = Encoding.UTF8.GetBytes(jwt["SecurityKey"]);

		        o.TokenValidationParameters = new TokenValidationParameters
		        {
			        ValidateIssuer = true,
			        ValidateAudience = true,
			        ValidateLifetime = true,
			        ValidateIssuerSigningKey = true,
			        ValidIssuer = jwt["Issuer"],
			        ValidAudience = jwt["Audience"],
			        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
		        };
	        });
}

// TODO: remove or disable if using Blazor client
void AddStandardAuthentication(IServiceCollection services)
{
	services
		.AddAuthentication(o =>
		{
			o.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
			o.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
			o.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
			o.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
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
		JwtBearerDefaults.AuthenticationScheme, // TODO: remove or disable if using SSR
		IdentityConstants.ApplicationScheme // TODO: remove or disable if using Blazor client
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
