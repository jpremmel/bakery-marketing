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
using Microsoft.AspNetCore.Http;


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

        public ActionResult Details(int id)
        {
            Flavor flavor = _db.Flavors
                .Include(t => t.Treats)
                .ThenInclude(join => join.Treat)
                .FirstOrDefault(f => f.FlavorId == id);
            return View(flavor);
        }

        public ActionResult Create(int id)
        {
            ViewBag.Treats = _db.Treats.ToList();
            ViewBag.TreatId = id;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Flavor flavor, List<Treat> Treats, int TreatId)
        {
            System.Console.WriteLine("CHECKBOX TREATS: ");
            foreach (Treat treat in Treats)
            {
                System.Console.WriteLine(">>>>>>>>>>>>>>>>>>> " + treat.Name);
            }
            System.Console.WriteLine("FLAVOR JUST CREATED: " + flavor.Name);
            foreach (TreatFlavor join in flavor.Treats)
            {
                System.Console.WriteLine(">>>>> " + join.Treat.Name);
            }
            // var selectedTreats = Request.Form.GetValues("SelectedFruits");
            // foreach(Treat treat in Treats)
            // {
            //     System.Console.WriteLine(">>>>>>>>>>" + treat.Name);
            // }
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            flavor.User = currentUser;
            _db.Flavors.Add(flavor);
            if (TreatId != 0)
            {
                _db.TreatFlavors.Add(new TreatFlavor() { FlavorId = flavor.FlavorId, TreatId = TreatId });
            }
            _db.SaveChanges();
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Edit(int id)
        {
            Flavor flavor = _db.Flavors
                .Include(t => t.Treats)
                .ThenInclude(join => join.Treat)
                .FirstOrDefault(f => f.FlavorId == id);
            return View(flavor);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Flavor flavor/*, int TreatId*/)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            flavor.User = currentUser;
            // if (TreatId != 0)
            // {
            //     _db.TreatFlavors.Add(new TreatFlavor() { TreatId = TreatId, FlavorId = flavor.FlavorId });
            // }
            _db.Entry(flavor).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Details", "Flavors", new { id = flavor.FlavorId });
        }

        public ActionResult Delete(int id)
        {
            Flavor flavor = _db.Flavors.FirstOrDefault(f => f.FlavorId == id);
            return View(flavor);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            Flavor flavor = _db.Flavors.FirstOrDefault(f => f.FlavorId == id);
            flavor.User = currentUser;
            _db.Flavors.Remove(flavor);
            _db.SaveChanges();
            return RedirectToAction("Index", "Account");
        }
    }
}