using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using EconomySheetUpdater.Controllers;
using NavigationRoutes;

namespace BootstrapMvcSample
{
    public class ExampleLayoutsRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapNavigationRoute <StartPageController>("Start", c => c.Index());
            routes.MapNavigationRoute<HomeController>("Register Payment", c => c.Index());
            routes.MapNavigationRoute<OverviewController>("Overview", c => c.IndexHelper());
            routes.MapNavigationRoute<WorksheetController>("Create new Month", c => c.WorksheetHelper());
            //routes.MapNavigationRoute<HomeController>("Register Payments", c => c.IndexAsync());

            //routes.MapNavigationRoute<ExampleLayoutsController>("Example Layouts", c => c.Starter())
            //      .AddChildRoute<ExampleLayoutsController>("Marketing", c => c.Marketing())
            //      .AddChildRoute<ExampleLayoutsController>("Fluid", c => c.Fluid())
            //      .AddChildRoute<ExampleLayoutsController>("Sign In", c => c.SignIn())
            //    ;
        }
    }
}
