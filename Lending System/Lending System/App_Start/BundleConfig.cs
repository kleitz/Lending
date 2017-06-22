using System.Web;
using System.Web.Optimization;

namespace Lending_System
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            // Css
            bundles.Add(new StyleBundle("~/Content/metro-css").Include(
                      "~/Content/metro-bootstrap.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/metro-css-full").Include(
                      "~/Content/metro-bootstrap.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/bootstrap-datetimepicker.min.css"));
            bundles.Add(new StyleBundle("~/Content/A-Custom/Login").Include(
                      "~/Content/A-Custom/Login/login-style-orange-button-only.css",
                      "~/Content/A-Custom/Login/login-style.css"));
            bundles.Add(new StyleBundle("~/Content/A-Custom/Tables").Include(
                      "~/Content/A-Custom/Tables/bootstrap.min.css",
                      "~/Content/A-Custom/Tables/dataTables.bootstrap.min.css",
                      "~/Content/A-Custom/Tables/responsive.bootstrap.min.css",
                      "~/Content/A-Custom/Tables/select2-bootstrap.css",
                      "~/Content/A-Custom/Tables/select2.min.css",
                      "~/Content/A-Custom/Tables/wizard-steps.css"));
            // Scripts
            bundles.Add(new ScriptBundle("~/bundles/validation").Include(
                      "~/Scripts/jquery.validate.min.js",
                      "~/Scripts/jquery.validate.unobtrusive.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/A-Custom/Url").Include(
                      "~/Scripts/A-Custom/Url/url.js"));
            bundles.Add(new ScriptBundle("~/bundles/A-Custom/Tables").Include(
                      "~/Scripts/A-Custom/Tables/jquery.dataTables.min.js",
                      "~/Scripts/A-Custom/Tables/dataTables.responsive.min.js",
                      "~/Scripts/A-Custom/Tables/dataTables.bootstrap.min.js",
                      "~/Scripts/A-Custom/Tables/responsive.bootstrap.min.js"));
            //Custom Bundles
            bundles.Add(new StyleBundle("~/Content/UI").Include(
                     "~/Content/UI/components/reset.css",
                     "~/Content/UI/components/site.css",
                     "~/Content/UI/components/container.css",
                     "~/Content/UI/components/grid.css",
                     "~/Content/UI/components/header.css",
                     "~/Content/UI/components/image.css",
                     "~/Content/UI/components/menu.css",
                     "~/Content/UI/components/divider.css",
                     "~/Content/UI/components/list.css",
                     "~/Content/UI/components/segment.css",
                     "~/Content/UI/components/dropdown.css",
                     "~/Content/UI/components/icon.css",
                     "~/Content/UI/content/transition.css"));
            bundles.Add(new ScriptBundle("~/bundles/UI").Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Content/UI/components/transition.js",
                    "~/Content/UI/components/dropdown.js",
                    "~/Content/UI/components/visibility.js"));

            // knockout js bundling
            bundles.Add(new ScriptBundle("~/bundles/knockoutjs").Include(
                "~/Scripts/knockout-{version}.js"));

            // ko global
            bundles.Add(new ScriptBundle("~/bundles/ko-global").Include(
                "~/Assets/kojs/app.js",
                "~/Assets/kojs/helper.js"));

            // toastr
            bundles.Add(new StyleBundle("~/bundles/toastr-style").Include("~/Content/toastr.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/toastr-script").Include("~/Scripts/toastr.min.js"));
        }
    }
}