using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Cmentarz.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/myScripts").Include(
                        "~/Scripts/jquery-3.1.1.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/superfish.js",
                        "~/Scripts/jquery.scrolltotop.js",
                        "~/Scripts/common.js",
                        "~/Scripts/jquery.flexslider-min.js",
                        "~/Scripts/jquery-ui-1.12.1.js",
                        "~/Scripts/jquery-ui-1.12.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-responsive.min.css",
                      "~/Content/bootstrappage.css",
                      "~/Content/flexslider.css",
                      "~/Content/site.css",
					  "~/Content/L.Control.Sidebar.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/themes/base/core.css",
                "~/Content/themes/base/autocomplete.css",
                "~/Content/themes/base/theme.css",
                "~/Content/themes/base/menu.css"));

			bundles.Add(new ScriptBundle("~/bundles/leafletDraw").Include(
						"~/Scripts/Leaflet.draw.js",
						"~/Scripts/Leaflet.Draw.Event.js",
						"~/Scripts/edit/handler/Edit.Poly.js",
						"~/Scripts/edit/handler/Edit.SimpleShape.js",
						"~/Scripts/edit/handler/Edit.Circle.js",
						"~/Scripts/edit/handler/Edit.Rectangle.js",
						"~/Scripts/draw/handler/Draw.Feature.js",
						"~/Scripts/draw/handler/Draw.Polyline.js",
						"~/Scripts/draw/handler/Draw.Polygon.js",
						"~/Scripts/draw/handler/Draw.SimpleShape.js",
						"~/Scripts/draw/handler/Draw.Rectangle.js",
						"~/Scripts/draw/handler/Draw.Circle.js",
						"~/Scripts/draw/handler/Draw.Marker.js",
						"~/Scripts/ext/TouchEvents.js",
						"~/Scripts/ext/LatLngUtil.js",
						"~/Scripts/ext/GeometryUtil.js",
						"~/Scripts/ext/LineUtil.Intersect.js",
						"~/Scripts/ext/Polyline.Intersect.js",
						"~/Scripts/ext/Polygon.Intersect.js",
						"~/Scripts/Control.Draw.js",
						"~/Scripts/Tooltip.js",
						"~/Scripts/Toolbar.js",
						"~/Scripts/draw/DrawToolbar.js",
						"~/Scripts/edit/EditToolbar.js",
						"~/Scripts/edit/handler/EditToolbar.Edit.js",
						"~/Scripts/edit/handler/EditToolbar.Delete.js",
						"~/Scripts/leaflet.ajax.js",
						"~/Scripts/leaflet.ajax.min.js",
						"~/Scripts/L.Control.Sidebar.js"));
		}
    }
}