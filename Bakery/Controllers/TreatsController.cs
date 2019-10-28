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

        public ActionResult Details(int id)
        {
            Treat treat = _db.Treats
                .Include(f => f.Flavors)
                .ThenInclude(join => join.Flavor)
                .FirstOrDefault(t => t.TreatId == id);
            return View(treat);
        }

        public ActionResult Create(int id)
        {
            ViewBag.Flavors = _db.Flavors.ToList();
            ViewBag.FlavorId = id;
            return View();
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
            return RedirectToAction("Details", "Treats", new { id = treat.TreatId });
        }

        public ActionResult Edit(int id)
        {
            Treat treat = _db.Treats
                .Include(f => f.Flavors)
                .ThenInclude(join => join.Flavor)
                .FirstOrDefault(t => t.TreatId == id);
            return View(treat);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Treat treat/*, int FlavorId*/)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            treat.User = currentUser;
            // if (FlavorId != 0)
            // {
            //     _db.TreatFlavors.Add(new TreatFlavor() { FlavorId = FlavorId, TreatId = treat.TreatId });
            // }
            _db.Entry(treat).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Delete(int id)
        {
            Treat treat = _db.Treats.FirstOrDefault(t => t.TreatId == id);
            return View(treat);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            Treat treat = _db.Treats.FirstOrDefault(t => t.TreatId == id);
            treat.User = currentUser;
            _db.Treats.Remove(treat);
            _db.SaveChanges();
            return RedirectToAction("Index", "Account");
        }
        
    }
}