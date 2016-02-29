using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetYou.Models;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using System.Net.Mime;

namespace BudgetYou.Controllers
{
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invitations
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            var invitations = db.Invitations.Include(i => i.Households).Where(u => user.HouseholdId == u.HouseholdId).ToList();

            return View(invitations);
        }

        // GET: Invitations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // GET: Invitations/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            var userHousehold = db.Households.Where(u => user.HouseholdId == u.Id).ToList();

            ViewBag.HouseholdId = new SelectList(userHousehold, "Id", "Name");

            return View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseholdId,JoinCode,ToEmail,Joined")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                var existingUser = db.Users.Where(u => u.Email == invitation.ToEmail).FirstOrDefault();
                Household household = db.Households.Find(user.HouseholdId);

                invitation.JoinCode = Guid.NewGuid();
                invitation.HouseholdId = household.Id;
                db.Invitations.Add(invitation);
                db.SaveChangesAsync();
                try
                {
                    //Build Email Message
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.To.Add(new MailAddress(invitation.ToEmail, invitation.ToEmail));
                    mailMessage.From = new MailAddress(user.Email, "From");
                    mailMessage.Subject = "Budget-You: Invitation to Join a Household";

                    
                    if (existingUser != null)
                    {
                        var callbackUrlForExistingUser = Url.Action("Create", "Households", new { guid = invitation.JoinCode}, protocol: Request.Url.Scheme);

                        string bodytext = String.Concat(@"<p>I would like to invite you to join my household <i> ", household.Name,
                                    " </i>in the Budget-You app budgeting system", invitation.JoinCode, "</p> <p><a href='"
                                    , callbackUrlForExistingUser, "'>Join</a></p>");
                        mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(bodytext, null, MediaTypeNames.Text.Html));

                    }
                    else
                    {
                        //var callbackUrl = Url.Action("Index", "Households", null, protocol: Request.Url.Scheme);
                        var callbackUrl = Url.Action("RegisterToJoinHousehold", "Account", new { inviteHouseholdId = invitation.HouseholdId, invitationId = invitation.Id, guid = invitation.JoinCode }, protocol: Request.Url.Scheme);

                        string html = String.Concat(@"<p>I would like to invite you to join my household <i> ", household.Name,
                                        " </i>in the Budget-You app budgeting system.</p> <p><a href='", callbackUrl, "'>Join</a></p>");


                        mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
                    }
                    //Initialise SmtpClient and send
                    SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                    var SendGridCredentials = db.SendgridCredentials.First();
                    NetworkCredential credentials = new NetworkCredential(SendGridCredentials.UserName, SendGridCredentials.Password);
                    smtpClient.Credentials = credentials;
                    smtpClient.Send(mailMessage);

                    return RedirectToAction("Index", "Invitations");

                }
                catch (Exception ex)
                {
                    ViewBag.Exception = ex.Message;
                    return View(invitation);
                }
            }
            return View(invitation);
        }
    
        

// GET: Invitations/Edit/5
public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            return View(invitation);
        }

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,JoinCode,ToEmail,Joined")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invitation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            return View(invitation);
        }

        // GET: Invitations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invitation invitation = db.Invitations.Find(id);
            db.Invitations.Remove(invitation);
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
