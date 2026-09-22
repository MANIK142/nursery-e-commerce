using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nursery.Web.Host.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class OrderController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> getallorder()
    {

    }
}
