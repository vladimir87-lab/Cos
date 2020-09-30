using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ControlOper
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
            name: "Login",
            url: "login",
            defaults: new { controller = "Home", action = "Login" }
             );

            routes.MapRoute(
            name: "Index",
            url: "",
            defaults: new { controller = "Home", action = "Index" }
             );

            routes.MapRoute(
            name: "Admin",
            url: "admin",
            defaults: new { controller = "Admin", action = "Index" }
             );
            routes.MapRoute(
            name: "Exit",
            url: "exit",
            defaults: new { controller = "Home", action = "Exit" }
             );
            routes.MapRoute(
            name: "ListManeger",
            url: "listmeneger",
            defaults: new { controller = "Admin", action = "ListManeger" }
             );
            routes.MapRoute(
            name: "Maneger",
            url: "maneger/{page}",
            defaults: new { controller = "Home", action = "Maneger", page = UrlParameter.Optional }
             );
            routes.MapRoute(
            name: "ListModerators",
            url: "listmoderators",
            defaults: new { controller = "Admin", action = "ListModerators" }
             );
            routes.MapRoute(
            name: "AddManeger",
            url: "addmaneger/{iduser}",
            defaults: new { controller = "Admin", action = "AddManeger" , iduser = UrlParameter.Optional }
             );

            routes.MapRoute(
            name: "DellManeger",
            url: "dellmaneger/{iduser}",
            defaults: new { controller = "Admin", action = "DellManeger", iduser = UrlParameter.Optional }
             );
            routes.MapRoute(
            name: "SetPassManeger2",
            url: "setmanegerpas",
            defaults: new { controller = "Admin", action = "SetPassManeger" }
             );
            routes.MapRoute(
            name: "SetPassManeger",
            url: "setmanegerpas/{iduser}",
            defaults: new { controller = "Admin", action = "SetPassManeger", iduser = UrlParameter.Optional }
             );
            routes.MapRoute(
            name: "OrderId",
            url: "maneger-ocenka/{idorder}",
            defaults: new { controller = "Home", action = "OrderId", idorder = UrlParameter.Optional }
             );
            routes.MapRoute(
            name: "OrderIdPost",
            url: "maneger-ocenka",
            defaults: new { controller = "Home", action = "OrderId"}
             );
            routes.MapRoute(
            name: "FilterOrder",
            url: "filterorder",
            defaults: new { controller = "Home", action = "FilterOrder" }
             );







        }
    }
}
