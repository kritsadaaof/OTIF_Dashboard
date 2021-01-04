using System.Web;
using System.Web.Optimization;

namespace OTIF
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

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css")); 

            bundles.Add(new ScriptBundle("~/Home/js").Include(
                          "~/Scripts/Home/js_Index.js"));

            bundles.Add(new ScriptBundle("~/HomeOview/js").Include(
                          "~/Scripts/Home/js_Overview.js"));

            bundles.Add(new ScriptBundle("~/Request/js").Include(
                          "~/Scripts/Request/js_Request.js"));

            bundles.Add(new ScriptBundle("~/ShopFloor/js").Include(
                          "~/Scripts/ShopFloor/js_RPShopFloor.js"));

            bundles.Add(new ScriptBundle("~/ShopFloorFst/js").Include(
                          "~/Scripts/ShopFloor/js_RPShopFloorFst.js"));

            bundles.Add(new ScriptBundle("~/Return/js").Include(
                          "~/Scripts/Return/js_Return.js"));

            bundles.Add(new ScriptBundle("~/Move/js").Include(
                          "~/Scripts/Move/js_Move.js"));
             
            bundles.Add(new ScriptBundle("~/Planner/js").Include(
                          "~/Scripts/Planner/js_Planner.js"));


            bundles.Add(new ScriptBundle("~/ChangePlan/js").Include(
                          "~/Scripts/Planner/js_ChangePlan.js"));

            bundles.Add(new ScriptBundle("~/CreateBar/js").Include(
                          "~/Scripts/CreateBar/js_CreateBar.js"));

            bundles.Add(new ScriptBundle("~/QCTester/js").Include(
                          "~/Scripts/QC/js_QC.js"));


            bundles.Add(new ScriptBundle("~/Packing/js").Include(
                          "~/Scripts/Packing/js_Packing.js"));

            bundles.Add(new ScriptBundle("~/Packing2/js").Include(
                          "~/Scripts/Packing/js_Packing2.js"));


            bundles.Add(new ScriptBundle("~/PackingCheck/js").Include( 
                          "~/Scripts/Packing/js_PackingCheck.js"));


            bundles.Add(new ScriptBundle("~/PackingCheck2/js").Include(
                          "~/Scripts/Packing/js_PackingCheck2.js"));

            bundles.Add(new ScriptBundle("~/MonitorTV1/js").Include(
                          "~/Scripts/MonitorMach/js_MonitorTV1.js"));
            bundles.Add(new ScriptBundle("~/MonitorTV2/js").Include(
                          "~/Scripts/MonitorMach/js_MonitorTV2.js"));
            bundles.Add(new ScriptBundle("~/MonitorTV3/js").Include(
                          "~/Scripts/MonitorMach/js_MonitorTV3.js"));
            bundles.Add(new ScriptBundle("~/MonitorTV4/js").Include(
                          "~/Scripts/MonitorMach/js_MonitorTV4.js"));

            bundles.Add(new ScriptBundle("~/PackingFG/js").Include(
                         "~/Scripts/DataFG/js_PackingFG.js"));


            bundles.Add(new ScriptBundle("~/PackingReceive/js").Include(
                         "~/Scripts/Packing/js_PackingReceive.js"));

        }
    }
}
