using System.Web.Optimization;

namespace PaylevenWebAppSharpDemo
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/style").Include("~/static/style/*.css"));
        }
    }
}
