using Hangfire;
using HangfireNLog.NLog;
using NLog.Targets;
using System.Collections.Generic;

namespace NLog.HangfireLayouts
{
    [Target("Hangfire")]
    public sealed class HangfireTarget : TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = this.Layout.Render(logEvent);

            var jobStorageConnection = JobStorage.Current.GetConnection();

            // I have to find where to store messages
            if (logEvent.Properties.TryGetValue(JobDecoratorLayoutRenderer.HANGFIRE_JOB_ID_PROPERTY_NAME, out object jobIdObj))
            {
                var jobId = (string)jobIdObj;

                using (var tran = jobStorageConnection.CreateWriteTransaction())
                {
                    var keyValuePairs = new[] {
                        new KeyValuePair<string, string>($"{logEvent.SequenceID}-message", logMessage),
                        new KeyValuePair<string, string>($"{logEvent.SequenceID}-ticks", logEvent.TimeStamp.Ticks.ToString())
                    };

                    tran.SetRangeInHash($"nlog-jobId:{jobId}", keyValuePairs);
                    tran.Commit();
                }
            }
        }
    }
}
