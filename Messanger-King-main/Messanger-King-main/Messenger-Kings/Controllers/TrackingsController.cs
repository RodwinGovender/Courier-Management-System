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
    public class TrackingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trackings
        public ActionResult Index()
        {
            var trackings = db.Trackings.Include(t => t.Order);
            return View(trackings.ToList());
        }

        // GET: Trackings/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tracking tracking = db.Trackings.Find(id);
            if (tracking == null)
            {
                return HttpNotFound();
            }
            return View(tracking);
        }


        public ActionResult DriverLoad()
        {
            var uid = User.Identity.GetUserId();


            var trackings = db.Trackings.Include(t => t.Order).Where(d => d.Order.Driver.Driver_ID == uid && d.Order.Bookings.BookStatus==true && d.Order.Bookings.paymentstatus == true);
            return View(trackings.ToList());
        }
        
        public ActionResult Tracking(string searchString)
        {
            

            var trackings = from s in db.Trackings
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                trackings = trackings.Where(s =>
               s.Track_ID.ToUpper().Contains(searchString.ToUpper()));


                return View(trackings.ToList());
            }


            trackings = trackings.Where(s =>
                   s.Track_ID == null);


            return View(trackings.ToList());
            
             // MK-5087448972

            
        }

           // GET: Trackings/Create
        public ActionResult Create()
        {
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID");
            return View();
        }

        // POST: Trackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Track_ID,Track_Message,Order_Status,Out_for_delivery,Pickup_Status,Delivery_Status,Order_ID")] Tracking tracking)
        {
            if (ModelState.IsValid)
            {
                db.Trackings.Add(tracking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID", tracking.Order_ID);
            return View(tracking);
        }

        // GET: Trackings/Edit/5
        public ActionResult Edit(int? id)
        {
            var trackID = (from i in db.Trackings
                           where i.Order_ID == id
                           select i.Track_ID).FirstOrDefault();



            if (trackID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tracking tracking = db.Trackings.Find(trackID);
            if (tracking == null)
            {
                return HttpNotFound();
            }
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID", tracking.Order_ID);
            return View(tracking);
        }

        // POST: Trackings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Track_ID,Track_Message,Order_Status,Out_for_delivery,Pickup_Status,Delivery_Status,Order_ID")] Tracking tracking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tracking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DeliveryList");
            }
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID", tracking.Order_ID);
            return View(tracking);
        }

        // GET: Trackings/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tracking tracking = db.Trackings.Find(id);
            if (tracking == null)
            {
                return HttpNotFound();
            }
            return View(tracking);
        }

        // POST: Trackings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tracking tracking = db.Trackings.Find(id);
            db.Trackings.Remove(tracking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MyTracking()
        {

            ViewBag.NoTrack = TempData["NoTrack"];

            return View();
        }

        // [Authorize Client]
        public ActionResult MyTrackingSearch(string id)
        {

            var track = db.Trackings.Find(id);
            if (track == null)
            {
                TempData["NoTrack"] = "Tracking Number Not Found. Please Ensure It Is Correct";
                return RedirectToAction("MyTracking");
            }
            TempData["NoTrack"] = "";
            return View(track);
        }

        public ActionResult Waybill(string id)
        {       

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tracking tracking = db.Trackings.Find(id);
            if (tracking == null)
            {
                return HttpNotFound();
            }

            return View(tracking);
        }

        public ActionResult WaybillList()
        {
            var orders = db.Trackings.Include(t => t.Order).Where(a => a.Track_Message == "The Order Has Been Approved!");



            return View(orders.ToList());
        }

        public ActionResult AllWaybills()
        {
            var orders = db.Trackings.Include(t => t.Order).Where(a => a.Track_Message != "");



            return View(orders.ToList());
        }

        public ActionResult WaybillStatusChange(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tracking tracking = db.Trackings.Find(id);
            if (tracking == null)
            {
                return HttpNotFound();
            }

            tracking.Track_Message = "Out for Pickup";
            db.Entry(tracking).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("WaybillList");

            
        }

        public ActionResult QrRedirect(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tracking tracking = db.Trackings.Find(id);
            if (tracking == null)
            {
                return HttpNotFound();
            }

          
            if (User.IsInRole("Driver"))
            {
                return RedirectToAction("Create", "ClientSignatures", new { id = id });
            }

            else if (User.IsInRole("Employee"))
            {

                if (tracking.Track_Message == "Order has been Picked up")
                {

                    return RedirectToAction("ManagePackage", "Waybills", new { id = id });
                }
                else if (tracking.Track_Message == "Order has arrived at Warehouse")
                {

                    return RedirectToAction("ManagePackageDispatch", "Waybills", new { id = id });
                }

                ViewBag.Error = "Package tracking error. Please scan next package";

                return RedirectToAction("Store", "Waybills");
            }

            else 
            
            {
                return RedirectToAction("MyTrackingSearch", new { id = id });
            }

            
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
