using System.Web;
using System.Web.Optimization;

namespace PollPrototype
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/site.css")
                .Include("~/Content/loading-bar.css")
                .Include("~/Content/nv.d3.css")
                .Include("~/Content/xeditable.css"));
        }
    }
}