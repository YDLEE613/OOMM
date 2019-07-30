namespace BootcampTrainee.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Security;

    /// <summary>
    /// This class manages GET and POST actions regarding Errors
    /// </summary>
    public class ErrorController : Controller
    {
        // Default
        public ActionResult Default()
        {
            // error message for default
            ViewBag.message = "Unexpected Error occured. Please try later";
            return View();
        }
        // 401
        public ActionResult UnAuthorized401()
        {
            // error message for access to unauthorized page
            ViewBag.message = "<strong style='font-weight:800'>Sorry, You are not authorized to access this page.</strong>";
            return View();
        }
        // 404
        public ActionResult NotFound404()
        {
            // error message for failure to find a page
            ViewBag.message = "<strong style='font-weight:800'>"+
                              "This Page is not available. We are working on this issue. Thank you for your patience.</strong>";
            return View();
        }

        // 500
        public ActionResult ServerError500()
        {
            // clear session
            Session.Clear();
            FormsAuthentication.SignOut();
            // error message for server error
            ViewBag.message = "<p><strong>Oops, Something went wrong. </strong>"+
                            "The server encountered an internal error or misconfiguration and was unable to complete your request.</p>"+
                            "<p>We are working on this! Thank you for your patience.</p>";
            return View();
        }
    }
}