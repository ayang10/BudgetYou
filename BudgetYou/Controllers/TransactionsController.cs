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

namespace BudgetYou.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            

            var transactions = db.Transactions.Include(t => t.Account).Include(t => t.Category);

            
            
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,Description,Date,Types,Amount,CategoryId,Reconciled,EntryId,ReconciledAmount")] Transaction transaction)
        {
            transaction.Date = new DateTimeOffset(DateTime.Now);

            if (ModelState.IsValid)
            {
                transaction.Date = new DateTimeOffset(DateTime.Now);

                transaction.EntryId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;

                var account = db.Accounts.FirstOrDefault(x => x.Id == transaction.AccountId);
                

                transaction.ReconciledAmount = transaction.Amount;

                transaction.Reconciled = true;

                if (transaction.Types == true)
                {
                    account.Balance += transaction.Amount;
                }

                if (transaction.Types == false)
                {
                    account.Balance -= transaction.Amount;
                }

                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,Description,TypeOfFunction,Date,Amount,CategoryId,Reconciled,EntryId,ReconciledAmount")] Transaction transaction)
        {
            transaction.Date = new DateTimeOffset(DateTime.Now);

            if (ModelState.IsValid)
            {
                transaction.Date = new DateTimeOffset(DateTime.Now);
                transaction.EntryId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;

                var originalBalance = db.Transactions.AsNoTracking().FirstOrDefault(o => o.Id == transaction.Id);
                var account = db.Accounts.FirstOrDefault(a => a.Id == transaction.AccountId);

                // Reverse original balance calculation
                if (transaction.Types == true)
                {
                    account.Balance = (account.Balance + originalBalance.Amount);
                }
                if (transaction.Types == false)
                {
                    account.Balance = (account.Balance - originalBalance.Amount);
                }

                // New edit balance calculation
                if (transaction.Types == true)
                {
                    account.Balance = (account.Balance - transaction.Amount);
                    db.SaveChanges();
                }
                if (transaction.Types == false)
                {
                    account.Balance = (account.Balance + transaction.Amount);
                    db.SaveChanges();
                }


                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);

            var account = db.Accounts.FirstOrDefault(x => x.Id == transaction.AccountId);

            if (transaction.Types == true)
            {
                account.Balance -= transaction.Amount;
            }
            if (transaction.Types == false)
            {
                account.Balance += transaction.Amount;
            }

            db.Transactions.Remove(transaction);
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
