using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Web.Helpers;
using System.Collections.Generic;
using NewLibrary.ForType;

namespace Website.Helpers
{
    public class CustomWebGrid : WebGrid
    {
        // Example
        // Set defaults etc.
        // could make Type version WebGrid<T> as alternative.
    }

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

        public static IHtmlString Auto(this WebGrid webgrid)
        {
            return webgrid.GetHtml(tableStyle: "gridTable", alternatingRowStyle: "gridAltRow" );
        }

        public static WebGrid WebGrid (this HtmlHelper helper, IEnumerable<dynamic> sourceList, string exclusions = "" )
        {
            var metaModel = sourceList.GetMetaModel();
            string[] columns = metaModel.GetMetaProperties(exclude: exclusions).Select(a => a.Name).ToArray();

            var grid = new WebGrid(source: sourceList, columnNames: columns); 
            return grid;
        }
    }
}
 