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
    public class TreatsController : Controller
    {
        private readonly BakeryContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TreatsController(UserManager<ApplicationUser> userManager, BakeryContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public ActionResult Create(int FlavorId)
        {
            ViewBag.Flavors = _db.Flavors.ToList();
            ViewBag.FlavorId = FlavorId;
            return View();
        }

        public ActionResult Details(int id)
        {
            Treat treat = _db.Treats
                .Include(f => f.Flavors)
                .ThenInclude(join => join.Flavor)
                .FirstOrDefault(t => t.TreatId == id);
            return View(treat);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Treat treat, int FlavorId)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            treat.User = currentUser;
            _db.Treats.Add(treat);
            if (FlavorId != 0)
            {
                _db.TreatFlavors.Add(new TreatFlavor() { TreatId = treat.TreatId, FlavorId = FlavorId});
            }
            _db.SaveChanges();
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Edit(int id)
        {


            return View();
        }

        [HttpPost]
        public ActionResult Edit(Treat treat)
        {


            return RedirectToAction("Index", "Account");
        }

        public ActionResult Delete(int id)
        {
            Treat treat = _db.Treats.FirstOrDefault(t => t.TreatId == id);
            return View(treat);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Treat treat = _db.Treats.FirstOrDefault(t => t.TreatId == id);
            _db.Treats.Remove(treat);
            _db.SaveChanges();
            return RedirectToAction("Index", "Account");
        }
        
    }
}