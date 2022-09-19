using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace EkipProjesi.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/calendarcss")
                 .Include("~/Content/global/vendor/fullcalendar/fullcalendar.css")
                 .Include("~/Content/global/vendor/bootstrap-touchspin/bootstrap-touchspin.css")
                 .Include("~/Content/assets/examples/css/apps/calendar.css")
                 );          
            bundles.Add(new ScriptBundle("~/bundles/momentjs")
              .Include("~/Content/global/vendor/moment/moment-with-locales.js")
              );
            bundles.Add(new StyleBundle("~/bundles/footablecss")
                .Include("~/Content/global/vendor/footable/footable.core.css")
                .Include("~/Content/assets/examples/css/tables/footable.css")
                );

            bundles.Add(new ScriptBundle("~/bundles/footablejs")
                .Include("~/Content/global/vendor/footable2/footable.js")
                //.Include("~/Content/global/vendor/footable2/footable.paging.js")
                //.Include("~/Content/global/vendor/footable2/footable.filtering.js")
                );
            bundles.Add(new StyleBundle("~/bundles/datepickercss")
             .Include("~/Content/global/vendor/bootstrap-datepicker/bootstrap-datepicker.css")
             );
            bundles.Add(new ScriptBundle("~/bundles/datepickerjs")
              .Include("~/Content/global/vendor/bootstrap-datepicker/bootstrap-datepicker.js")
              .Include("~/Content/global/vendor/bootstrap-datepicker/bootstrap-datepicker.tr.min.js")
              );
            bundles.Add(new ScriptBundle("~/Content/global/vendor/anketjs")
                .Include("~/Content/global/vendor/jquery-wizard-master/jquery-wizard.js")
                .Include("~/Content/global/vendor/matchheight/jquery.matchHeight-min.js")
                .Include("~/Content/global/js/Plugin/jquery-wizard.js")
                .Include("~/Content/global/vendor/asprogress/jquery-asProgress.min.js")
                );
            BundleTable.EnableOptimizations = true;
        }
    }
}