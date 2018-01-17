using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebAuthorUser1.Models
{
    public class ContactModels
    {
        public void doSomething()
        {
            try
            {
                doNothing();
            }
            catch (Exception e)
            {
                TelemetryClient telemetry = new TelemetryClient();
                telemetry.TrackException(e);
            }
        }

        private void doNothing()
        {
            int.Parse("thisisntanumber");
        }
    }
}