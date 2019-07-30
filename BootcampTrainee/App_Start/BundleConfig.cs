using System.Web;
using System.Web.Optimization;

namespace BootcampTrainee
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.dataTables.js",
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/ckeditor/ckeditor/js",
                         //"~/Scripts/bootstrap-rating/bootstrap-rating.js",
                         //"~/Scripts/bootstrap-rating/bootstrap-rating.min.js",
                        "~/Scripts/Chart.min.js",
                        "~/Scripts/js/star-rating.js"
                        ));

            //"~/Scripts/datables.extensions/FixedColumns/js/dataTables.fixedColumns.min.js"
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      //"~/Content/bootstrap-rating.css",
                      "~/Content/site.css",
                      "~/Content/css/star-rating.css",
                      "~/Content/css/jquery.dataTables.css"));
        }
    }
}
