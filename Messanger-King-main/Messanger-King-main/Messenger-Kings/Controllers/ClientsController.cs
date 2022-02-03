using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Messenger_Kings.Models;
using Microsoft.AspNet.Identity;

namespace Messenger_Kings.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Clients/Details/5
        public ActionResult Details()
        {
            var uid = User.Identity.GetUserId();
            var id = uid;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }
        [ChildActionOnly]
        public ActionResult Link()
        {

            return PartialView();
        }
        // GET: Clients/Edit/5
        public ActionResult Edit()
        {
            var uid = User.Identity.GetUserId();
            var id = uid;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientCat_ID = new SelectList(db.ClientCategories, "ClientCat_ID", "ClientCat_Type", client.ClientCat_ID);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Client_ID,Client_IDNo,Client_Name,Client_Surname,User_Name,Client_Cellnumber,Client_Address,Client_Email,Client_Tellnum,ClientCat_ID")] Client client)
        {
            if (ModelState.IsValid)
            {

                var uid = User.Identity.GetUserId();


                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                //if()
                return RedirectToAction("Details");
            }
            ViewBag.ClientCat_ID = new SelectList(db.ClientCategories, "ClientCat_ID", "ClientCat_Type", client.ClientCat_ID);
            return View(client);
        }

        // GET: Clients/Delete/5

        // POST: Clients/Delete/5
   

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
