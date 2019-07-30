namespace BootcampTrainee.Filters
{
    using System.Linq;
    using System.Web.Mvc;

    public class MustBeInRoleAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (this.Roles.Split(',').Any(filterContext.HttpContext.User.IsInRole))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                //string ReturnURL = filterContext.RequestContext.HttpContext.Request.Path.ToString();
                //filterContext.Controller.TempData.Add("Warning",
                //    $"you must be logged into any account to access this resource, you are not currently logged in at all");
                //filterContext.Controller.TempData.Add("ReturnURL", ReturnURL);

                System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary();

                dict.Add("Controller", "Error");
                dict.Add("Action", "UnAuthorized401");
                filterContext.Result = new RedirectToRouteResult(dict);
            }

        }
    }
}