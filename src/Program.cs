using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HealthDashboard
{
	public class Program
	{
		public static void Main(string[] args)
		{
			string assembly = default;
			try
			{
				assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

				var configuration = BuildConfiguration();

				Log.Logger = new LoggerConfiguration()
					.ReadFrom
					.Configuration(configuration)
					.CreateLogger();

				Log.Information("{assemblyName} starting.", assembly);

				CreateHostBuilder(args)
					.Build()
					.Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "{assemblyName} failed to start correctly.", assembly);
				throw;
			}
			finally
			{
				Log.Information("{assemblyName} has stopped.", assembly);
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) => Host
			.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((hostingContext, config) => {
				_ = config
					.AddJsonFile("appsettings.json", false, true)
					.AddJsonFile($"logSettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
					.AddJsonFile("hostsettings.json", true, true)
					.AddJsonFile($"hostsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
				if (hostingContext.HostingEnvironment.IsDevelopment())
				{
					_ = config.AddUserSecrets<Program>();
				}
				else
				{
					config.AddEnvironmentVariables("HealthDashboard_");
				}

			})
			.UseSerilog()
			.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

		private static IConfiguration BuildConfiguration()
		{
			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var configBuilder = new ConfigurationBuilder()
				.AddJsonFile("logSettings.json", false, true)
				.AddJsonFile($"logSettings.{environment}.json", true, true);
			return configBuilder.Build();
		}
	}
}
