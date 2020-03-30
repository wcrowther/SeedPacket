using Examples.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Examples.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString ActiveToggle(this HtmlHelper helper, string controllerName, string actionName = null,
                                            string activeClass = "active", string inactiveClass = "inactive")
        {
            var currentController = helper.ViewContext.RouteData.Values["controller"].ToString();
            var currentAction = helper.ViewContext.RouteData.Values["action"].ToString();
            bool isActive = controllerName == currentController && (actionName == null || actionName == currentAction);
            string returnString = isActive ? activeClass : inactiveClass;

            return new HtmlString(returnString);
        }
    }
}
