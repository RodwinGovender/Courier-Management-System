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
    public class ClientSignaturesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClientSignatures
        public ActionResult Index()
        {
            var clientSignatures = db.ClientSignatures.Include(c => c.Driver).Include(c => c.Order);
            return View(clientSignatures.ToList());
        }

        // GET: ClientSignatures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientSignature clientSignature = db.ClientSignatures.Find(id);
            if (clientSignature == null)
            {
                return HttpNotFound();
            }
            return View(clientSignature);
        }

        // GET: ClientSignatures/Create
        public ActionResult Create(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tracking = db.Trackings.Find(id);
            if (tracking == null)
            {
                return HttpNotFound();
            }

         
            ClientSignature cs = new ClientSignature();

            if (tracking.Track_Message == "Out for Pickup")
            {
                cs.Recipient_Type = "Client";
            }

            else if (tracking.Track_Message == "Package has been dispatched from Warehouse")
            {
                cs.Recipient_Type = "Recipient";
            }
            else
            {
                return RedirectToAction("DeliveryList","Orders");
            }

            ViewBag.DeliveryDate = tracking.Order.Bookings.Book_DeliveryDate.ToShortDateString();
            ViewBag.PickupDate = tracking.Order.Bookings.Book_PickupDate.ToShortDateString();

            cs.Sign_Date = DateTime.Now;
            cs.Order_ID = tracking.Order_ID;
            cs.Driver_ID = tracking.Order.Driver_ID;

            ViewBag.Trackings = tracking;

            

            return View(cs);
        }

        // POST: ClientSignatures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Signaturee_ID,Sign_Date,MySignature,Order_ID,Driver_ID,SignedBy,Recipient_Type")] ClientSignature clientSignature)
        {
            if (ModelState.IsValid)
            {
                db.ClientSignatures.Add(clientSignature);
                           
                Tracking tracking = db.Trackings.Where(t => t.Order_ID == clientSignature.Order_ID).FirstOrDefault();

                if (clientSignature.Recipient_Type == "Client")
                {
                    tracking.Track_Message = "Order has been Picked up";
                }
                else if (clientSignature.Recipient_Type == "Recipient")
                {
                    tracking.Track_Message = "Order has been Delivered";

                }

                if (clientSignature.SignedBy == null || clientSignature.SignedBy == "")
                {

                    clientSignature.SignedBy = "Confirmed by Driver";
                    
                }

                db.Entry(tracking).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("DeliveryList","Orders");
            }

            ViewBag.Driver_ID = new SelectList(db.Drivers, "Driver_ID", "Driver_IDNo", clientSignature.Driver_ID);
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID", clientSignature.Order_ID);
            return View(clientSignature);
        }

        // GET: ClientSignatures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientSignature clientSignature = db.ClientSignatures.Find(id);
            if (clientSignature == null)
            {
                return HttpNotFound();
            }
            ViewBag.Driver_ID = new SelectList(db.Drivers, "Driver_ID", "Driver_IDNo", clientSignature.Driver_ID);
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID", clientSignature.Order_ID);
            return View(clientSignature);
        }

        // POST: ClientSignatures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Signaturee_ID,Sign_Date,MySignature,Order_ID,Driver_ID")] ClientSignature clientSignature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientSignature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Driver_ID = new SelectList(db.Drivers, "Driver_ID", "Driver_IDNo", clientSignature.Driver_ID);
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID", clientSignature.Order_ID);
            return View(clientSignature);
        }

        // GET: ClientSignatures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientSignature clientSignature = db.ClientSignatures.Find(id);
            if (clientSignature == null)
            {
                return HttpNotFound();
            }
            return View(clientSignature);
        }


        public ActionResult MySignature(string id)
        {

            var track = (from b in db.Trackings where b.Track_ID == id select b).FirstOrDefault();
            var clientSignatures = db.ClientSignatures.Include(c => c.Driver).Include(c => c.Order).Where(z => z.Order_ID == track.Order_ID);
            if (clientSignatures == null)
            {
                return HttpNotFound();
            }
            ViewBag.TrackID = id;

            return View(clientSignatures.ToList());




        }

        // POST: ClientSignatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientSignature clientSignature = db.ClientSignatures.Find(id);
            db.ClientSignatures.Remove(clientSignature);
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
