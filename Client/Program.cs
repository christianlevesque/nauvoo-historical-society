using System;
using System.Net.Http;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using Client;
using Client.Tools.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

builder.Services
       .AddOptions()
       .Configure<ApiOptions>(o =>
       {
	       o.BaseUrl = builder.Configuration.GetSection("Api:BaseUrl").Value;
       })
       .AddAuthorizationCore()
       .AddBlazoredLocalStorage()
       .AddMudServices()
       .AddScoped(sp =>
       {
	       var options = sp.GetRequiredService<IOptions<ApiOptions>>()
	                       .Value;
	       var uriKind = options.BaseUrl.Contains("http")
		                     ? UriKind.Absolute
		                     : UriKind.Relative;

	       return new HttpClient { BaseAddress = new Uri(options.BaseUrl, uriKind) };
       })
       .AutoinjectServicesFromAssembly(typeof(Program).Assembly);

await builder.Build()
             .RunAsync();