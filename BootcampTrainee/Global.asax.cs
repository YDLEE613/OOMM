using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace BootcampTrainee
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        protected void Application_AcquireRequestState(Object sender, EventArgs e)
        {
            string UserName = Session["AUTHUserName"] as string;
            string Sessroles = Session["AUTHRole"] as string;

            if (string.IsNullOrEmpty(UserName))
            {
                return;
            }
            GenericIdentity genericIdentity = new GenericIdentity(UserName, "");
            
            if(Sessroles == null)
            {
                Sessroles = "";
            }

            string[] roles = Sessroles.Split(',');

            GenericPrincipal genericPrincipal = new GenericPrincipal(genericIdentity, roles);
            HttpContext.Current.User = genericPrincipal;
        }
    }
}
