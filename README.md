# Health Dashboard

This dashboard uses [ASP.NET Health Checks][hc] and the [Health Checks UI][hcui] to create a health check dashboard for MTD's services.

## Basic Configuration

The applicaiton logs health history to a database. You must add a connection string.

```json
"ConnectionStrings": {
	"HealthCheckConnection": "Data Source=YOUR_SQL_SERVER.YOUR_DOMAIN.com;Initial Catalog=YOUR_DB_NAME;Integrated Security=False;Persist Security Info=True;User ID=YOUR_USER_ID;Password=YOUR_PW;MultipleActiveResultSets=True;"
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
			"apiKey": "YOUR_KEY"
		}
	  }
	]
  }
```

[hc]: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0
[hcui]: https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
[sl]: https://serilog.net/
[seq]: https://datalust.co/seq


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