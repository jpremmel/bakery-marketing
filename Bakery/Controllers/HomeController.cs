using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Bakery.Controllers
{
    public class HomeController : Controller
    {
        private readonly BakeryContext _db;

        public HomeController(BakeryContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            ViewBag.Treats = _db.Treats.ToList();
            ViewBag.Flavors = _db.Flavors.ToList();
            return View();
        }
    }
}