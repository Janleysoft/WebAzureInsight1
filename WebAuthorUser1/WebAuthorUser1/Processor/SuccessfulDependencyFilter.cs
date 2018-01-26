using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAuthorUser1.Processor
{
    public class SuccessfulDependencyFilter : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        // You can pass values from .config
        public string MyParamFromConfigFile { get; set; }

        // Link processors to each other in a chain.
        public SuccessfulDependencyFilter(ITelemetryProcessor next)
        {
            this.Next = next;
        }
        public void Process(ITelemetry item)
        {
            var request = item as DependencyTelemetry;

            if (request != null && request.Duration.TotalMilliseconds < 200)
            {
                return;
            }
            this.Next.Process(item);
        }

        // Example: replace with your own modifiers.
        private void ModifyItem(ITelemetry item)
        {
            item.Context.Properties.Add("app-version", "1.0" + MyParamFromConfigFile);
        }
    }
}