using Microsoft.AspNetCore.Mvc;

namespace sales_mvc.Controllers
{
    public class SellersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
