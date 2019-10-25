using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
        //needs to return list of all treats and flavors to view
      return View();
    }
  }
}