using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Messenger_Kings.Models;
using Microsoft.AspNet.Identity;

namespace Messenger_Kings.Controllers
{
    
    public class DriversController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Drivers
        public ActionResult Index()
        {
            return View(db.Drivers.ToList());
        }

        // GET: Drivers/Details/5
        public ActionResult Details()
        {

            var uid = User.Identity.GetUserId();
            var id = uid;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        //// GET: Drivers/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Drivers/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Driver_ID,Driver_IDNo,Driver_Name,Diver_Image,Driver_Surname,Driver_CellNo,Driver_Address,Driver_Email")] Driver driver)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Drivers.Add(driver);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(driver);
        //}

        // GET: Drivers/Edit/5
        public ActionResult Edit()
        {

            var uid = User.Identity.GetUserId();
            var id = uid;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Driver_ID,Driver_IDNo,Driver_Name,Diver_Image,Driver_Surname,Driver_CellNo,Driver_Address,Driver_Email")] Driver driver, HttpPostedFileBase filelist)
        {
            if (ModelState.IsValid)
            {

                if (filelist != null && filelist.ContentLength > 0)
                {
                    driver.Diver_Image = ConvertToBytes(filelist);
                }

                db.Entry(driver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(driver);
        }

        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            BinaryReader reader = new BinaryReader(file.InputStream);
            return reader.ReadBytes((int)file.ContentLength);
        }

        // GET: Drivers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Driver driver = db.Drivers.Find(id);
            db.Drivers.Remove(driver);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RatingDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            ViewBag.DriverId = id;

            var comments = db.Comments.Where(d => d.Driver_ID == id).ToList();
            ViewBag.Comments = comments;

            var ratings = db.Comments.Where(d => d.Driver_ID == id).ToList();

            var rating = 0;
            var ratingCount = 0;
            int? ratingSum = 0;
            if (ratings.Count() > 0)
            {
                ratingSum = ratings.Sum(d => d.Rating);
                ratingCount = ratings.Count();
                rating = ((int)ratingSum / ratingCount);
                var totalRating = decimal.Parse(rating.ToString());
                ViewBag.TotalRating = totalRating;
            }


            return View(driver);
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
