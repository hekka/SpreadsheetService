using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EconomySheetUpdater.Controllers;
using NavigationRoutes;

namespace EconomySheetUpdater
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Clear();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapRoute("", "{controller}/{action}/{id}", new { controller = "StartPage", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("Overview", "{controller}/{action}/{id}", new { controller = "Overview", action = "IndexHelper", id = UrlParameter.Optional });
            routes.MapRoute("Worksheet", "{controller}/{action}/{id}", new { controller = "Worksheet", action = "WorksheetHelper", id = UrlParameter.Optional });
        }
    }
}