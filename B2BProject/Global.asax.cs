using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace B2BProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static CouponExpirationService _couponExpirationService;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // CouponExpirationService baþlat
            _couponExpirationService = new CouponExpirationService();
            _couponExpirationService.Start();
        }

        protected void Application_End()
        {
            // CouponExpirationService durdur
            _couponExpirationService.Stop();
        }
    }
}
