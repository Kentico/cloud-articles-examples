﻿using System.Web.Mvc;
using System.Web.Routing;
using DancingGoat.Localization;

namespace DancingGoat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var route = routes.MapRoute(
                name: "CoffeesCatalog",
                url: "{language}/product-catalog/coffees/{action}/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Coffees", action = "Index", urlSlug = UrlParameter.Optional },
                constraints: new { language = new LanguageConstraint() }
            );
            route.RouteHandler = new LocalizedMvcRouteHandler(LanguageClient.DEFAULT_LANGUAGE);

            route = routes.MapRoute(
                name: "BrewersCatalog",
                url: "{language}/product-catalog/brewers/{action}/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Brewers", action = "Index", urlSlug = UrlParameter.Optional },
                constraints: new { language = new LanguageConstraint() }
            );
            route.RouteHandler = new LocalizedMvcRouteHandler(LanguageClient.DEFAULT_LANGUAGE);

            route = routes.MapRoute(
                name: "AccessoriesCatalog",
                url: "{language}/product-catalog/accessories/{action}/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Accessories", action = "Index", urlSlug = UrlParameter.Optional },
                constraints: new { language = new LanguageConstraint() }
            );
            route.RouteHandler = new LocalizedMvcRouteHandler(LanguageClient.DEFAULT_LANGUAGE);

            route = routes.MapRoute(
                name: "Articles",
                url: "{language}/articles",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Articles", action = "Index" },
                constraints: new { language = new LanguageConstraint() }
            );
            route.RouteHandler = new LocalizedMvcRouteHandler(LanguageClient.DEFAULT_LANGUAGE);
            route = routes.MapRoute(
                name: "Article",
                url: "{language}/articles/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Articles", action = "Show", urlSlug = "" },
                constraints: new { language = new LanguageConstraint() }
);
            route.RouteHandler = new LocalizedMvcRouteHandler(LanguageClient.DEFAULT_LANGUAGE);

            route = routes.MapRoute(
                name: "LocalizedContent",
                url: "{language}/{controller}/{action}/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Home", action = "Index", urlSlug = UrlParameter.Optional },
                constraints: new { language = new LanguageConstraint() }
            );
            route.RouteHandler = new LocalizedMvcRouteHandler(LanguageClient.DEFAULT_LANGUAGE);

            // Display a custom view when no route is found
            routes.MapRoute(
                name: "Error",
                url: "{*url}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Errors", action = "NotFound" }
            );

        }
    }
}