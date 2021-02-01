# Health Dashboard

[![Build Status](https://dev.azure.com/cumtd/MTD/_apis/build/status/HealthDashboard/HealthDashboard%20-%20Main?branchName=main)](https://dev.azure.com/cumtd/MTD/_build/latest?definitionId=30&branchName=main)

This dashboard uses [ASP.NET Health Checks][hc] and the [Health Checks UI][hcui]
to create a health check dashboard.
This application is a then wrapper around Health Checks UI.
It adds authentication, logging, DB storage, and custom CSS to the out of the box experiance.


## Basic Configuration

Custom configuraiton should not be committed to source control.
By default, this application is configured to read from User Secrets in development
and Environmental variables or a `healthchecks.json` file in production.

For IIS, environmental variables should be prefixed with HealthDashboard_ and nested JSON should be indicated with a __.
For example `HealthDashboard_AzureAD__Instance`. The exception to this is the connection strings.
They should not be prefixed. For example `ConnectionStrings__HealthCheckConnection`.

For the `healthchecks.json` file, simply include the JSON to add to the configuration.
This should be used to define health checks and not to store sensitive connection string or OIDC items. See [Adding a Health Check
](#Adding-a-Health-Check)

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
	"Authority": "https://login.microsoftonline.com/{YOUR_TENNANT_ID}/v2.0/",
	"TenantId": "{YOUR_TENNANT_ID}",
	"ClientId": "{YOUR_CLIENT_ID}",
	"ClientSecret": "{YOUR_CLIENT_SECRET}",
	"Domain": "{YOUR_DOMAIN}"
}
```

## Adding a Health Check
These should be defined in `healthchecks.json`.
```json
{
	"HealthChecksUI": {
		"HealthChecks": [
			{
				"Name": "Site Name",
				"Uri": "https://example.com/healthz"
			}
		]
	}
}
```

## Adding Web Hooks
You can define web hooks in the "HealthChecksUI" section of the config. An example of a Teams webhook is as follows.

```json
"HealthChecksUI": {
	"Webhooks": [
      {
        "Name": "Teams",
        "Uri": "{Your Webhook URL}",
        "Payload": "{\"@context\": \"http://schema.org/extensions\",\"@type\": \"MessageCard\",\"themeColor\": \"e10027\",\"title\": \"[[LIVENESS]] has failed!\",\"text\": \"[[FAILURE]]. Click **Learn More** to view Health Dashboard!\",\"potentialAction\": [{\"@type\": \"OpenUri\",\"name\": \"Learn More\",\"targets\": [{ \"os\": \"default\", \"uri\": \"https://foo.bar/\" }]}]}"
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
