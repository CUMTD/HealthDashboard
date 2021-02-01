using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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
			var dashboardConfig = _configuration.Get<DashboardConfig>();

			_ = services.AddSingleton(dashboardConfig);

			_ = services.Configure<IISServerOptions>(options => options.AutomaticAuthentication = false);

			_ = services.AddHsts(options =>
			{
				options.Preload = true;
				options.IncludeSubDomains = true;
				options.MaxAge = TimeSpan.FromDays(365 * 2);
			});

			_ = services.AddRouting(options =>
			{
				options.LowercaseUrls = true;
				options.AppendTrailingSlash = true;
			});

			_ = services.Configure<IISServerOptions>(options => options.AutomaticAuthentication = false);

			_ = services.AddHsts(options =>
			{
				options.Preload = true;
				options.IncludeSubDomains = true;
				options.MaxAge = TimeSpan.FromDays(365 * 2);
			});

			_ = services
				.AddHealthChecksUI()
				.AddSqlServerStorage(dashboardConfig.ConnectionStrings.HealthCheckConnection);

			_ = services
				.AddAuthentication(options => 
				{
					options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
				})
				.AddCookie()
				.AddOpenIdConnect(options => {
					ConfigurationBinder.Bind(_configuration, "AzureAd", options);
					options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				});

			_ = services.AddAuthorization();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				_ = app.UseDeveloperExceptionPage();
			}
			else
			{
				_ = app.UseHsts();
				_ = app.UseHttpsRedirection();
			}

			_ = app.UseStaticFiles();

			_ = app.UseRouting();

			_ = app.UseAuthentication();
			_ = app.UseAuthorization();

			_ = app
				.UseEndpoints(endpoints => _ = endpoints.MapHealthChecksUI(options => {
					options.UIPath = "/";
					_ = options.AddCustomStylesheet("wwwroot/css/mtd.css");
				})
				.RequireAuthorization()
			);
		}
	}
}
