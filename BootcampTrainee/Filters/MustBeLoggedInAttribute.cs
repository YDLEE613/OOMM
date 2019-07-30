namespace BootcampTrainee.Filters
{
    using System.Web.Mvc;

    public class MustBeLoggedInAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // Call base class to allow user into the action method
                base.OnAuthorization(filterContext);
            }
            else
            {
                //string ReturnURL = filterContext.RequestContext.HttpContext.Request.Path.ToString();
                //filterContext.Controller.TempData.Add("Warning",
                //    $"you must be logged into any account to access this resource, you are not currently logged in at all");
                //filterContext.Controller.TempData.Add("ReturnURL", ReturnURL);


                System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary();

                dict.Add("Controller", "User");
                dict.Add("Action", "UserLogIn");
                filterContext.Result = new RedirectToRouteResult(dict);
            }
        }
    }
}