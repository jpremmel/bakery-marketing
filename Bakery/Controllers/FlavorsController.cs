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
    [Authorize]
    public class FlavorsController : Controller
    {
        private readonly BakeryContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public FlavorsController(UserManager<ApplicationUser> userManager, BakeryContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(Flavor flavor)
        {
            _db.Flavors.Add(flavor);
            _db.SaveChanges();
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Edit(int id)
        {

            
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Flavor flavor)
        {


            return RedirectToAction("Index", "Account");
        }

        public ActionResult Delete(int id)
        {
            Flavor flavor = _db.Flavors.FirstOrDefault(f => f.FlavorId == id);
            return View(flavor);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Flavor flavor = _db.Flavors.FirstOrDefault(f => f.FlavorId == id);
            _db.Flavors.Remove(flavor);
            _db.SaveChanges();
            return RedirectToAction("Index", "Account");
        }
    }
}