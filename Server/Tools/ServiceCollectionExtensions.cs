using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Server.Tools;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddServerServices(this IServiceCollection services, IHostEnvironment env)
	{
		return services
			.AddRoutingServices(env)
			.AddServerOptions();
	}

	private static IServiceCollection AddRoutingServices(this IServiceCollection services, IHostEnvironment env)
	{
		services
			.AddRouting()
			.AddControllers();

		// TODO: remove or disable if using Blazor client
		services
			.AddRazorPages(o =>
			{
				o.Conventions.AuthorizeFolder("/");
			})
			.AddRazorRuntimeCompilation();

		if (env.IsDevelopment())
		{
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(policy =>
				{
					policy
						.AllowAnyOrigin()
						.AllowAnyHeader()
						.AllowAnyMethod();
				});
			});
		}

		return services;
	}

	private static IServiceCollection AddServerOptions(this IServiceCollection services)
	{
		// TODO: remove or disable if using SSR
		// Changes the invalid model state response
		services.Configure<ApiBehaviorOptions>(
			options =>
			{
				options.InvalidModelStateResponseFactory = context => new UnprocessableEntityObjectResult(context.ModelState);
			}
		);

		// TODO: remove or disable if using SSR
		services.Configure<DataProtectionTokenProviderOptions>(o =>
		{
			o.TokenLifespan = TimeSpan.FromDays(30);
		});

		return services;
	}
}