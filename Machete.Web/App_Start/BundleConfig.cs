using System.Web;
using System.Web.Optimization;

namespace Machete.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Content/bundles/runtime*",
                        "~/Content/bundles/polyfills*",
                        "~/Content/bundles/styles*",
                        "~/Content/bundles/scripts*",
                        "~/Content/bundles/vendor*",
                        "~/Content/bundles/main*"
                        ));
            bundles.Add(new StyleBundle("~/bundles/angular-css").Include(
                        "~/Content/bundles/styles*"
                ));
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

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/Site.css",
                "~/Content/confirm/jquery.alerts.css",
                "~/Content/dataTables/demo_table_jui.css",
                "~/Content/jquery-ui-1.8.6.custom.css"));

            bundles.Add(new StyleBundle("~/Content/reportcss").Include(
                    "~/Content/jquery.jqplot.css"
                ));
        }
    }
}