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
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard(int? id)
        {

            DashboardViewModels model = new DashboardViewModels();

            var user = db.Users.Find(User.Identity.GetUserId()); //get user Id
            

            model.Households = db.Users.FirstOrDefault(a => a.UserName == User.Identity.Name).Household; //get users of Household
            model.Transactions = db.Transactions.Where(a => a.Account.HouseholdId == user.HouseholdId).OrderByDescending(a => a.Id).Take(6).ToList(); //get in descending order transactions
            model.Accounts = db.Accounts.Where(a => a.HouseholdId == model.Households.Id).ToList(); //get all account for this household
            //model.Invitations = db.Invitations.Where(a => a.ToEmail == User.Identity.Name).ToList(); //

            var getBudget = db.Budgets.Where(a => user.HouseholdId == a.HouseholdId).ToList(); //get List of existing budgets in this household
            var CurrentBudget = db.Budgets.First(a => a.HouseholdId == model.Households.Id); //Get currentbudget for this 

            if (id == null) //there will be a id already, so this step will more than likely be skipped
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
               
           
          

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateChart(int BudgetId)
        {

            return RedirectToAction("Dashboard", new { id = BudgetId });//retrieve data back to dashboard view
        }


        //[HttpGet]
        //public ActionResult ChangeTransactionDate(int month, int year)
        //{

        //    DateTime startDate = new DateTime(year, month, 1, 0, 0, 0);
        //    DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59, 999);


        //    Transaction transactions = new Transaction();
        //    DateTime counterDate = startDate;




        //    return RedirectToAction("Dashboard", new { id = month, year });
        //}

       

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
