using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetYou.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;

namespace BudgetYou.Controllers
{

    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: Households
        //public ActionResult Index()
        //{

        //    return View(db.Households.ToList());
        //}


        // GET: Households
        [Authorize]
        public ActionResult Index()
        {


            var user = db.Users.Find(User.Identity.GetUserId());
            Household household = db.Households.Find(user.HouseholdId);
            if (household == null)
            {
                return RedirectToAction("Create", "Households");
            }


            return View(household);
        }

        // GET: Households/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Name")] Household household)
        {


            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                if (user.HouseholdId == null)
                {
                    db.Households.Add(household);
                    db.SaveChanges();

                    var getHousehold = db.Households.FirstOrDefault(h => h.Name == household.Name);
                    user.HouseholdId = getHousehold.Id;
                    db.SaveChanges();

                    return RedirectToAction("Index", new { id = getHousehold.Id });
                }
                else
                {
                    return RedirectToAction("Index", new { id = user.HouseholdId });
                }
            }


            return View();
        }

        [HttpGet]
        //GET: Households/Join

        public ActionResult Joins(Guid guid)
        {

            return View(guid);
        }

        [HttpPost]
        [Authorize]
        //POST: Households/Join
        public async Task<ActionResult> Joins(Household model, Guid guid)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                    var currUserEmail = User.Identity.GetUserName();
                    var invite = db.Invitations.FirstOrDefault(c => c.JoinCode == guid && c.ToEmail == currUserEmail);
                    if (invite != null)
                    {
                        user.HouseholdId = invite.HouseholdId;
                        db.Invitations.Remove(invite);
                      
                        db.SaveChanges();
                        return RedirectToAction("Index", new { id = user });
                    }
                    else
                    {
                        TempData["errorMessage"] = "An error has occurred";
                        return RedirectToAction("Create", "Households");
                    }
            
            }
            TempData["errorMessage"] = "An error has occurred";
            return RedirectToAction("Create", "Households");
        }

        // GET: Households/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }
        

        // GET: Households/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        public ActionResult Leave()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            user.HouseholdId = null;
            db.SaveChanges();

            return RedirectToAction("Create", "Households");
        }


        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }





    }
}
