using System.Web;
using System.Web.Optimization;

namespace restaurant
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/Admin/js").Include(
                      "~/Scripts/jquery.easing.min.js",
                      "~/Scripts/jquery.min.js",
                      "~/Scripts/bootstrap.bundles.min.js",
                      "~/Scripts/chart.min.js",
                      "~/Scripts/chart-area-demo.min.js",
                      "~/Scripts/chart-pie-demo.min.js",
                      "~/Scripts/sb-admin.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/style.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Admin/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/sb-admin.css",
                      "~/Content/font-awesome.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Error/css").Include(
                      "~/Content/Error.css"));

            bundles.Add(new ScriptBundle("~/Error/js").Include(
                      "~/Scripts/Error.js"));
        }
    }
}
