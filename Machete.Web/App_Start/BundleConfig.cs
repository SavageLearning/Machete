using System.Web;
using System.Web.Optimization;

namespace Machete.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.7.1.js",
                        "~/Scripts/jquery-ui-1.8.11.js",
                        "~/Scripts/jquery.form.js",
                        "~/Scripts/jquery.dataTables-1.9.0.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery.alerts.js",
                        "~/Scripts/jquery.lightbox-0.5.js",
                        "~/Scripts/jquery.ui.datepicker.js",
                        "~/Scripts/console-log.js",
                        "~/Scripts/validation_hack.js",
                        "~/Scripts/jquery.dataTables.plugins.js",    
                        "~/Scripts/ui.datetimepicker.js",
                        "~/Scripts/maskedinput-1.3.js",
                        "~/Scripts/macheteLayout.js",
                        "~/Scripts/jquery.macheteUI-0.1.js",
                        "~/Scripts/jquery.everytime-1.2.0.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/reports").Include(
                    "~/Scripts/jquery.jqplot.js",
                    "~/Scripts/jqplot.cursor.js",
                    "~/Scripts/jqplot.dateAxisRenderer.js",
                    "~/Scripts/jqplot.highligher.js",
                    "~/Scripts/jqplot.pieRenderer.js",
                    "~/Scripts/reports.js"
                ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/Site.css",
                "~/Content/confirm/jquery.alerts.css",
                "~/Content/dataTables/demo_table_jui.css",
                "~/Content/jquery-ui-1.8.6.custom.css"));

            bundles.Add(new StyleBundle("~/Content/reportcss").Include(
                    "~/Content/jquery.jqplot.css"
                ));

                        //"~/Content/themes/base/jquery.ui.core.css",
                        //"~/Content/themes/base/jquery.ui.resizable.css",
                        //"~/Content/themes/base/jquery.ui.selectable.css",
                        //"~/Content/themes/base/jquery.ui.accordion.css",
                        //"~/Content/themes/base/jquery.ui.autocomplete.css",
                        //"~/Content/themes/base/jquery.ui.button.css",
                        //"~/Content/themes/base/jquery.ui.dialog.css",
                        //"~/Content/themes/base/jquery.ui.slider.css",
                        //"~/Content/themes/base/jquery.ui.tabs.css",
                        //"~/Content/themes/base/jquery.ui.datepicker.css",
                        //"~/Content/themes/base/jquery.ui.progressbar.css",
                        //"~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}