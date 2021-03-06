using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Messenger_Kings.Models;

namespace Messenger_Kings.Controllers
{
   
    public class BankCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BankCategories
        public ActionResult Index()
        {
            return View(db.BankCategories.ToList());
        }

        // GET: BankCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankCategory bankCategory = db.BankCategories.Find(id);
            if (bankCategory == null)
            {
                return HttpNotFound();
            }
            return View(bankCategory);
        }

        // GET: BankCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BankCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankCat_ID,Bank_Name")] BankCategory bankCategory)
        {
            if (ModelState.IsValid)
            {
                db.BankCategories.Add(bankCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bankCategory);
        }

        // GET: BankCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankCategory bankCategory = db.BankCategories.Find(id);
            if (bankCategory == null)
            {
                return HttpNotFound();
            }
            return View(bankCategory);
        }

        // POST: BankCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BankCat_ID,Bank_Name")] BankCategory bankCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bankCategory);
        }

        // GET: BankCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankCategory bankCategory = db.BankCategories.Find(id);
            if (bankCategory == null)
            {
                return HttpNotFound();
            }
            return View(bankCategory);
        }

        // POST: BankCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankCategory bankCategory = db.BankCategories.Find(id);
            db.BankCategories.Remove(bankCategory);
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
