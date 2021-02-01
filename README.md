# Health Dashboard

This dashboard uses [ASP.NET Health Checks][hc] and the [Health Checks UI][hcui] to create a health check dashboard for MTD's services.

## Basic Configuration

### Database

The applicaiton logs health history to a database. You must add a connection string.

```json
"ConnectionStrings": {
	"HealthCheckConnection": "Data Source={YOUR_SQL_SERVER.YOUR_DOMAIN.COM};Initial Catalog={YOUR_DB_NAME};Integrated Security=False;Persist Security Info=True;User ID={YOUR_USER_ID};Password={YOUR_PW};MultipleActiveResultSets=True;"
}
```

### Authentication

The application is configured to authenticate with [Azure Active Directory][azure].
You must add the following Azure AD section to the application's configuartion.
You can get the Client values by registering an App in your Azure Active Directory.
The steps outlined in [Microsoft's documentation][oidc-setup] may be helpful.

```json
"AzureAd": {
	"Instance": "https://login.microsoftonline.com/",
	"Authority": "https://login.microsoftonline.com/{YOUR_TENNANT_ID}/v2.0/",
	"TenantId": "{YOUR_TENNANT_ID}",
	"ClientId": "{YOUR_CLIENT_ID}",
	"CallbackPath": "/signin-oidc",
	"ClientSecret": "{YOUR_CLIENT_SECRET}",
	"Domain": "{YOUR_DOMAIN}"
}
```

## Adding a Health Check

```json
"HealthChecksUI": {
	"HealthChecks": [
		{
			"Name": "Site Name",
			"Uri": "https://example.com/healthz"
		}
	]
}
```

## Logging

This application uses [Serilog][sl] for logging and is intended to use a [Seq][seq] sink.
In development, it points to a localhost Seq instance (http://localhost:5341).
To enable Seq, in production add it to the `logSettings.json` config:

```json
"Serilog": {
	"WriteTo": [
	  {
		"Name": "Seq",
		"Args": {
			"serverUrl": "https://seq.example.com/",
			"apiKey": "{YOUR_KEY}"
		}
	  }
	]
  }
```

## License
Copyright 2021 Champaign-Urbana Masss Transit District

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.


[hc]: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0
[hcui]: https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
[azure]: https://azure.microsoft.com/en-us/services/active-directory/
[oidc-setup]: https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-aspnet-core-webapp#option-2-register-and-manually-configure-your-application-and-code-sample
[sl]: https://serilog.net/
[seq]: https://datalust.co/seq
