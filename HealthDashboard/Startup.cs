using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HealthDashboard
{
	public class Startup
	{

		public readonly IConfiguration _configuration;
		public readonly IWebHostEnvironment _environment;

		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_environment = environment ?? throw new ArgumentNullException(nameof(environment));
		}

		public void ConfigureServices(IServiceCollection services)
		{
			_ = services.Configure<IISServerOptions>(options => options.AutomaticAuthentication = false);

			_ = services.AddHsts(options =>
			{
				options.Preload = true;
				options.IncludeSubDomains = true;
				options.MaxAge = TimeSpan.FromDays(365 * 2);
			});

			_ = services
				.AddHealthChecksUI()
				.AddSqlServerStorage(_configuration.GetConnectionString("HealthCheckConnection"));

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				_ = app.UseDeveloperExceptionPage();
			}

			_ = app.UseStaticFiles();

			_ = app.UseRouting();

			_ = app.UseEndpoints(endpoints => _ = endpoints.MapHealthChecksUI(options => {
				options.UIPath = "/";
				_ = options.AddCustomStylesheet("wwwroot/css/mtd.css");
			}));
		}
	}
}
