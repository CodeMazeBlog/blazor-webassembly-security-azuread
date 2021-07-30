using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWasmHostedB2C.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddHttpClient("BlazorWasmHostedB2C.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
				.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

			// Supply HttpClient instances that include access tokens when making requests to the server project
			builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorWasmHostedB2C.ServerAPI"));

			builder.Services.AddMsalAuthentication(options =>
			{
				builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
				options.ProviderOptions.DefaultAccessTokenScopes.Add("https://codemazeb2c.onmicrosoft.com/7949ee12-56d0-4150-9473-6357c4a04b60/BlazorHostedB2CServer.Access");
			});

			await builder.Build().RunAsync();
		}
	}
}
