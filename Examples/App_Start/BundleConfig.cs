using System.Web;
using System.Web.Optimization;

namespace Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                          "~/Scripts/bootstrap.js",
                          "~/Scripts/respond.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/ScriptApp").Include(
                          "~/ScriptApp/Examples.js",
                          "~/ScriptApp/OLD-Teams.js"
                      ));

            //bundles.Add(new ScriptBundle("~/bundles/Teams").Include(
            //                  "~/ScriptApp/_footballIcon.js",
            //                  "~/ScriptApp/teams.js"
            //              ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                          "~/Content/bootstrap.css",
                          "~/Content/grid.css",
                          "~/Content/site.css"
                      ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
