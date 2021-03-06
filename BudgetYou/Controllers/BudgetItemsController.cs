﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetYou.Models;
using Microsoft.AspNet.Identity;

namespace BudgetYou.Controllers
{
    [Authorize]
    public class BudgetItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BudgetItems
        [Authorize]
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var budgetItems = db.BudgetItems.Where(t => t.Budget.HouseholdId == user.HouseholdId).Include(b => b.Category).ToList();

            Household household = db.Households.Find(user.HouseholdId);
            if (household == null)
            {
                return RedirectToAction("Create", "Households");
            }

            return View(budgetItems);
        }

        //// GET: BudgetItems/Details/5
        //[Authorize]
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BudgetItem budgetItem = db.BudgetItems.Find(id);
        //    if (budgetItem == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(budgetItem);
        //}

        // GET: BudgetItems/Create
        [Authorize]
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            var getBudget = db.Budgets.Where(u => user.HouseholdId == u.HouseholdId).ToList();

            ViewBag.BudgetId = new SelectList(getBudget, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: BudgetItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BudgetId,CategoryId,Amount")] BudgetItem budgetItem)
        {
            if (ModelState.IsValid)
            {
                db.BudgetItems.Add(budgetItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name", budgetItem.BudgetId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", budgetItem.CategoryId);
            return View(budgetItem);
        }

        // GET: BudgetItems/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            //check for authorization
            var user = db.Users.Find(User.Identity.GetUserId());
            BudgetItem budgetItem = db.BudgetItems.FirstOrDefault(x => x.Id == id);
            Budget budget = db.Budgets.FirstOrDefault(x => x.Id == budgetItem.BudgetId);
            Household household = db.Households.FirstOrDefault(x => x.Id == budget.HouseholdId);

            if (!household.Members.Contains(user))
            {
                return RedirectToAction("Unauthorized", "Error");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //BudgetItem budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            var getBudget = db.Budgets.Where(u => user.HouseholdId == u.HouseholdId).ToList();
            
            ViewBag.BudgetId = new SelectList(getBudget, "Id", "Name", budgetItem.BudgetId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", budgetItem.CategoryId);
            return View(budgetItem);
        }

        // POST: BudgetItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BudgetId,CategoryId,Amount")] BudgetItem budgetItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name", budgetItem.BudgetId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", budgetItem.CategoryId);
            return View(budgetItem);
        }

        // GET: BudgetItems/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            BudgetItem budgetItem = db.BudgetItems.FirstOrDefault(x => x.Id == id);
            Budget budget = db.Budgets.FirstOrDefault(x => x.Id == budgetItem.BudgetId);
            Household household = db.Households.FirstOrDefault(x => x.Id == budget.HouseholdId);

            if (!household.Members.Contains(user))
            {
                return RedirectToAction("Unauthorized", "Error");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //BudgetItem budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            return View(budgetItem);
        }

        // POST: BudgetItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            BudgetItem budgetItem = db.BudgetItems.FirstOrDefault(x => x.Id == id);
            Budget budget = db.Budgets.FirstOrDefault(x => x.Id == budgetItem.BudgetId);
            Household household = db.Households.FirstOrDefault(x => x.Id == budget.HouseholdId);

            if (!household.Members.Contains(user))
            {
                return RedirectToAction("Unauthorized", "Error");
            }
            //BudgetItem budgetItem = db.BudgetItems.Find(id);
            db.BudgetItems.Remove(budgetItem);
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
