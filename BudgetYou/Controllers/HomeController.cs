using BudgetYou.Models;
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
            
            //DashboardViewModels model = new DashboardViewModels();
            //model.Household = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;



            return View(/*model*/);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}