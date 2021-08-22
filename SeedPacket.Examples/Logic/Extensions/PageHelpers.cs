using Microsoft.AspNetCore.Mvc.Rendering;

namespace SeedPacket.Examples.Logic.Extensions
{
    public static class PageHelpers
    {
        // Usage: <li class='@Html.IsActive("/Index")'>
        public static string IsActive(this IHtmlHelper html, string route, string cssClass = "active")
        {
            var routeData = html?.ViewContext?.RouteData?.Values["Page"]?.ToString();

            return routeData == route ? cssClass : "";
        }
    }
}
