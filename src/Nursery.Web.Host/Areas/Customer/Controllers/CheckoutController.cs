using Microsoft.AspNetCore.Mvc;

namespace Nursery.Web.Host.Areas.Customer.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
