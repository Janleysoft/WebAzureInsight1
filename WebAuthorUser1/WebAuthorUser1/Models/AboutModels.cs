using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAuthorUser1.Models
{
    public class AboutModels
    {
        public void saySomething()
        {
            try
            {

                sayNothing();
            }
            catch (Exception e)
            {
                TelemetryClient telemetry = new TelemetryClient();
                telemetry.TrackException(e);
            }
        }

        private void sayNothing()
        {
            throw new DivideByZeroException();
        }
    }
}