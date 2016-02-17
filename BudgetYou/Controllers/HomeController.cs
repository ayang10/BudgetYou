using BudgetYou.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BudgetYou.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            
            DashboardViewModels model = new DashboardViewModels();

            //var UserHouseholdId = db.Users.Find(User.Identity.GetUserId()).HouseholdId;
            //model.Household = db.Households.Find(UserHouseholdId);
            model.Households = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;
            model.Transactions = db.Transactions.OrderByDescending(i => i.Id).Take(8).ToList();
            model.Accounts = db.Accounts.Where(i => i.HouseholdId == model.Households.Id).ToList();
            model.Invitations = db.Invitations.Where(i => i.ToEmail == User.Identity.Name).ToList();
            model.Budgets = db.Budgets.FirstOrDefault(u => u.HouseholdId == model.Households.Id);

           

            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}