using System.Web;
using System.Web.Optimization;

namespace BudgetYou
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

            bundles.Add(new StyleBundle("~/theme/css").Include(
                     "~/theme/css/bootstrap.css",
                     "~/theme/css/font-awesome.min.css",
                     "~/theme/css/less-style.css",
                     "~/theme/css/style.css"));

            bundles.Add(new StyleBundle("~/theme/js").Include(
                      "~/theme/js/jquery.js",
                      "~/theme/js/bootstrap.min.js",
                      "~/theme/js/respond.min.js",
                      "~/theme/js/html5shiv.js",
                      "~/theme/js/jquery-ui.min.js",
                      "~/theme/js/bootstrap-datetimepicker.min.js",
                      "~/theme/js/wysihtml5-0.3.0.js",
    "~/theme/js/prettify.js", 
   "~/theme/js/bootstrap-wysihtml5.min.js", 
   
         "~/theme/js/jquery.flot.min.js", 
           "~/theme/js/jquery.flot.stack.min.js" ,
            "~/theme/js/jquery.flot.pie.min.js",
              "~/theme/js/jquery.flot.resize.min.js",
          
           
            "~/theme/js/jquery.validate.js" ,
         
            "~/theme/js/jquery.steps.min.js",
          
            "~/theme/js/jquery.knob.js", 
              
           "~/theme/js/jquery.sparklines.js",
               
             "~/theme/js/jquery.slimscroll.min.js", 
               
             "~/theme/js/jquery.dataTables.min.js", 
               
            "~/theme/js/jquery.prettyPhoto.js", 
              
           "~/theme/js/jquery.rateit.min.js",
            "~/theme/js/moment.min.js" ,
                     "~/theme/js/fullcalendar.min.js",
                   
                 "~/theme/js/jquery.gritter.min.js",
      "~/theme/js/custom.notification.js",
      "~/theme/js/custom.js" 




                      ));

        }
    }
}
