using System.Web;
using System.Web.Optimization;

namespace eBillingSuite
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Assets/PixelAdmin/css/").Include(
						"~/assets/PixelAdmin/stylesheets/bootstrap.min.css",
                        "~/assets/PixelAdmin/stylesheets/buttons.dataTables.css",
                        "~/assets/PixelAdmin/stylesheets/pixel-admin.min.css",
						"~/assets/PixelAdmin/stylesheets/widgets.min.css",
						"~/assets/PixelAdmin/stylesheets/pages.min.css",
						"~/assets/PixelAdmin/stylesheets/rtl.min.css",
						"~/assets/PixelAdmin/stylesheets/themes.min.css"));

			bundles.Add(new ScriptBundle("~/Assets/PixelAdmin/js/").Include(
						"~/assets/SH/AjaxForms.js",
						"~/assets/PixelAdmin/javascripts/bootstrap.min.js",
						"~/assets/PixelAdmin/javascripts/pixel-admin.min.js",
						"~/assets/SH/jqDataTableExtension.js"));

            //bundles.Add(new ScriptBundle("~/Assets/Morris").Include(
            //    "~/assets/PixelAdmin/javascripts/jquery.min.js",
            //    "~/assets/PI/morris/morris.min.js",
            //    "~/assets/PI/raphael-min.js"
            //    ));

            //bundles.Add(new StyleBundle("~/Assets/Morris/css/").Include(
            //    "~/assets/PI/morris/morris.css"));

            BundleTable.EnableOptimizations = false;
		}
	}
}