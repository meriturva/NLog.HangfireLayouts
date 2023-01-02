using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobLogs
{
    /// <summary>
    /// Provides extension methods to setup Hangfire.JobLogs
    /// </summary>
    public static class GlobalConfigurationExtensions
    {
        /// <summary>
        /// Configures Hangfire to use JobLogs.
        /// </summary>
        /// <param name="configuration">Global configuration</param>
        public static IGlobalConfiguration UseJobLogs(this IGlobalConfiguration configuration)
        {
            return configuration;
        }
    }
}
