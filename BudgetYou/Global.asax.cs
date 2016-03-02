using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BudgetYou
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

        }
        protected void Application_BeginRequest()
        {
            var ci = CultureInfo.GetCultureInfo("en-US");

            if (Thread.CurrentThread.CurrentCulture.DisplayName == ci.DisplayName)
            {
                ci = CultureInfo.CreateSpecificCulture("en-US");
                ci.NumberFormat.CurrencyNegativePattern = 1;
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
        }


    }
}
