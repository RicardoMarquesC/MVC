using System.Web;
using System.Web.Optimization;

namespace PixelAdminMvc5
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Assets/PixelAdmin/css/").Include(
						"~/assets/PixelAdmin/stylesheets/bootstrap.min.css",
						"~/assets/PixelAdmin/stylesheets/pixel-admin.min.css",
						"~/assets/PixelAdmin/stylesheets/widgets.min.css",
						"~/assets/PixelAdmin/stylesheets/pages.min.css",
						"~/assets/PixelAdmin/stylesheets/rtl.min.css",
						"~/assets/PixelAdmin/stylesheets/themes.min.css"));

			bundles.Add(new ScriptBundle("~/Assets/PixelAdmin/js/").Include(
						"~/assets/PixelAdmin/javascripts/bootstrap.min.js",
						"~/assets/PixelAdmin/javascripts/pixel-admin.min.js"));

			BundleTable.EnableOptimizations = true;
		}
	}
}