using BudgetYou.Models;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
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

            var user = db.Users.Find(User.Identity.GetUserId());

            model.Households = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;
            model.Transactions = db.Transactions.Where(t => t.Account.HouseholdId == user.HouseholdId).OrderByDescending(i => i.Id).Take(6).ToList();
            model.Accounts = db.Accounts.Where(i => i.HouseholdId == model.Households.Id).ToList();
            model.Invitations = db.Invitations.Where(i => i.ToEmail == User.Identity.Name).ToList();
            model.Budgets = db.Budgets.Where(b => b.HouseholdId == model.Households.Id).ToList();

          

            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
