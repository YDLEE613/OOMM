namespace BootcampTrainee.Controllers
{
    using System.Web.Mvc;
    using BootcampTrainee.Filters;

    /// <summary>
    /// This class manages GET and POST actions for index page
    /// </summary>
    public class HomeController : Controller
    {
        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult Index()
        {
            return View();
        }
    }
}