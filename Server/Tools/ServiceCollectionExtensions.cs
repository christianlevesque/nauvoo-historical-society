using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Server.Tools;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddServerServices(this IServiceCollection services)
	{
		services
			.AddRouting()
			.AddRazorPages(o => o.Conventions.AuthorizeFolder("/"));

		return services
			.AutoinjectServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
	}
}