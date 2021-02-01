using System.Collections.Generic;

namespace HealthDashboard
{
	public class ConnectionStringsConfig
	{
		public string HealthCheckConnection { get; set; }
	}

	public class HealthCheckConfig
	{
		public string Name { get; set; }
		public string Uri { get; set; }
	}

	public class HealthChecksUIConfig
	{
		public int EvaluationTimeinSeconds { get; set; }
		public int MinimumSecondsBetweenFailureNotifications { get; set; }
		public IEnumerable<HealthCheckConfig> HealthChecks { get; set; }
	}

	public class DashboardConfig
	{
		public ConnectionStringsConfig ConnectionStrings { get; set; }
		public HealthChecksUIConfig HealthChecksUI { get; set; }
	}
}
