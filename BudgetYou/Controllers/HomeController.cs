using BudgetYou.Models;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BudgetYou.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Dashboard(int? id, int? low)
        {

            DashboardViewModels model = new DashboardViewModels(); //object of DashboardViewModels

            var user = db.Users.Find(User.Identity.GetUserId()); //get user Id
            

            model.Households = db.Users.FirstOrDefault(a => a.UserName == User.Identity.Name).Household; //get users of Household
            model.Transactions = db.Transactions.Where(a => a.Account.HouseholdId == user.HouseholdId).OrderByDescending(a => a.Id).Take(6).ToList(); //get in descending order transactions
            model.Accounts = db.Accounts.Where(a => a.HouseholdId == model.Households.Id).ToList(); //get all account for this household
            //model.Invitations = db.Invitations.Where(a => a.ToEmail == User.Identity.Name).ToList(); //

            var getBudget = db.Budgets.Where(a => user.HouseholdId == a.HouseholdId).ToList(); //get List of existing budgets in this household

            if (model.Households.Budgets.Count == 0)
            {
                return RedirectToAction("Create", "Budgets");

            }
            

                var CurrentBudget = db.Budgets.First(a => a.HouseholdId == model.Households.Id); //Get currentbudget for this 

            

            if (id == null) 
            {
                
                ViewBag.BudgetId = new SelectList(getBudget, "Id", "Name", CurrentBudget.Id); //Viewbag to get list of current existing budget
                model.GetBudgetId = CurrentBudget.Id; //assign current budget id to existing var to hold the value
                model.Budgets = CurrentBudget; //assign the current budget to show on chart
             
            }
            else
            {
                ViewBag.BudgetId = new SelectList(getBudget, "Id", "Name", id); //dispaly current existing budget in household
                model.GetBudgetId = (int)id; //assign id to GetBudgetId
                model.Budgets = db.Budgets.First(a => a.Id == id); //get budget assign to id
             
            }

            var currentDate = DateTime.Now;
          
            model.begin = new DateTime(currentDate.Year, currentDate.Month, 1);
            ////model.end = new DateTime(currentDate.Year, currentDate.Month,DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
            model.end = currentDate;

            //int currentAmount = 100;
            //    if (low == null)
            //    {
            //        model.setLowBalance = currentAmount;

            //    }
            //    else
            //    {
            //        currentAmount = (int)low;
            //        model.setLowBalance = currentAmount;
            //    }
                


            return View(model);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateChart(int BudgetId)
        {

            return RedirectToAction("Dashboard", new { id = BudgetId });//retrieve data back to dashboard view
        }



        //public ActionResult ChangeTransactionDate(DateTime startDate, DateTime endDate)
        //{
        //    DashboardViewModels model = new DashboardViewModels();

        //    //var currentDate = DateTime.Now;

        //    //model.Transactions = db.Transactions.Where(t => t.Date >= model.begin && t.Date <= model.end).ToList();
        //    model.begin = startDate;
        //    model.end = endDate;

        //    return RedirectToAction("Dashboard", model);
        //}

        //[HttpPost]
        //[Authorize]
        //public ActionResult setBalance(int setLowBalance)
        //{
           
        //    return RedirectToAction("Dashboard", new { low = setLowBalance});

        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your about page.";

            return View();
        }

       
    }
}
