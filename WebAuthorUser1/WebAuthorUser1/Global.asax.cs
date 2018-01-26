using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebAuthorUser1.Processor;

namespace WebAuthorUser1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.DefaultNamespaces.Add("WebAuthorUser1.Controllers");//set default namespace

            //Developer mode
            // TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = true;

            //Telemetry Processor
            var builder = TelemetryConfiguration.Active.TelemetryProcessorChainBuilder;
            builder.Use((next) => new SuccessfulDependencyFilter(next));
            //If you have more processors:
           // builder.Use((next) => new AnotherProcessor(next));
            builder.Build();

            //Telemetry Initializers                                                                                          
            TelemetryConfiguration.Active.TelemetryInitializers
            .Add(new MyTelemetryInitializer());
          //  TelemetryConfiguration.Active.TelemetryInitializers.Clear();
         
        }
    }
}
