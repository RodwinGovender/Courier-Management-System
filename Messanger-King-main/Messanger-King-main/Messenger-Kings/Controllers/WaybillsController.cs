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
    public class WaybillsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Waybills
        public ActionResult Index(string option, string search)
        {
            var allPackages = db.Waybills.Include(w => w.Order);
            var warehousePackages = db.Waybills.Include(w => w.Order).Where(x => x.Status_ID != 7);


            if (option == "All Packages")
            {
                //Index action method will return a view with a student records based on what a user specify the value in textbox  
                return View(allPackages.Where(x => x.Track_ID == search || search == null || search == "").ToList());
            }
            else if (option == "Packages In Warehouse")
            {
                return View(warehousePackages.Where(x => x.Track_ID == search || search == null || search == "").ToList());
            }
            else
            {
                return View(allPackages.Where(x => x.Track_ID == search || search == null || search == "").ToList());
            }


        }

        // GET: Waybills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Waybill waybill = db.Waybills.Find(id);
            if (waybill == null)
            {
                return HttpNotFound();
            }
            return View(waybill);
        }

        // GET: Waybills/Create
        public ActionResult Create()
        {
           
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID");
            return View();
        }

        // POST: Waybills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Waybill_ID,No_Packages_CheckedIN,No_Packages_CheckedOut,Date_Packages_CheckedOut,Date_Packages_CheckedIN,Order_ID,Driver_ID")] Waybill waybill)
        {
            if (ModelState.IsValid)
            {
                db.Waybills.Add(waybill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID", waybill.Order_ID);
            return View(waybill);
        }

        // GET: Waybills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Waybill waybill = db.Waybills.Find(id);
            if (waybill == null)
            {
                return HttpNotFound();
            }
           
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID", waybill.Order_ID);
            return View(waybill);
        }

        // POST: Waybills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Waybill_ID,No_Packages_CheckedIN,No_Packages_CheckedOut,Date_Packages_CheckedOut,Date_Packages_CheckedIN,Order_ID,Driver_ID")] Waybill waybill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(waybill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
          
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID", waybill.Order_ID);
            return View(waybill);
        }

        // GET: Waybills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Waybill waybill = db.Waybills.Find(id);
            if (waybill == null)
            {
                return HttpNotFound();
            }
            return View(waybill);
        }

        // POST: Waybills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Waybill waybill = db.Waybills.Find(id);
            db.Waybills.Remove(waybill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        

        public ActionResult ManagePackage(string id)
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

            Waybill waybill = new Waybill();

            waybill.Order_ID = tracking.Order_ID;
            waybill.Track_ID = tracking.Track_ID;

            ViewBag.Trackings = tracking;

            ViewBag.DeliveryDate = tracking.Order.Bookings.Book_DeliveryDate.ToShortDateString();
            ViewBag.PickupDate = tracking.Order.Bookings.Book_PickupDate.ToShortDateString();

            ViewBag.Status_ID = new SelectList(db.Status.Where(u => !u.Status_ID.Equals(7)).ToList(), "Status_ID", "Storage_Status");

            //ViewBag.Status_ID = new SelectList(db.Status, "Status_ID", "Storage_Status");
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID");

           
                waybill.Date_Packages_CheckedIN = DateTime.Now.Date.ToShortDateString();


            return View(waybill);
        }


       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManagePackage([Bind(Include = "Waybill_ID,Date_Packages_CheckedOut,Date_Packages_CheckedIN,Order_ID,Track_ID,Status_ID")] Waybill waybill)
        {
            if (ModelState.IsValid)
            {

               

                Tracking tracking = db.Trackings.Where(t => t.Order_ID == waybill.Order_ID).FirstOrDefault();

              
                    tracking.Track_Message = "Order has arrived at Warehouse";


                db.Waybills.Add(waybill);
                db.Entry(tracking).State = EntityState.Modified;   
                db.SaveChanges();

                TempData["Success"] = "You have successfully updated the status of this package, please Scan a new package or Type in a Tracking number";
                return RedirectToAction("Store");
            }


          
            ViewBag.Status_ID = new SelectList(db.Status, "Status_ID", "Storage_Status", waybill.Order_ID);
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID", waybill.Order_ID);
            return View(waybill);
        }


        public ActionResult ManagePackageDispatch(string id)
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

            Waybill waybill = db.Waybills.Where(t => t.Track_ID == id).FirstOrDefault();

            waybill.Order_ID = tracking.Order_ID;
            waybill.Track_ID = tracking.Track_ID;

            ViewBag.DeliveryDate = tracking.Order.Bookings.Book_DeliveryDate.ToShortDateString();
            ViewBag.PickupDate = tracking.Order.Bookings.Book_PickupDate.ToShortDateString();

            ViewBag.Trackings = tracking;
            ViewBag.Status_ID = new SelectList(db.Status, "Status_ID", "Storage_Status");
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID");

           

            waybill.Date_Packages_CheckedOut = DateTime.Now.Date.ToShortDateString();


            return View(waybill);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManagePackageDispatch([Bind(Include = "Waybill_ID,Date_Packages_CheckedOut,Date_Packages_CheckedIN,Order_ID,Track_ID,Status_ID")] Waybill waybill)
        {
            if (ModelState.IsValid)
            {

                waybill.Status_ID = 7;

                Tracking tracking = db.Trackings.Where(t => t.Order_ID == waybill.Order_ID).FirstOrDefault();
                

                tracking.Track_Message = "Package has been dispatched from Warehouse";

                var date = (from d in db.Orders
                            where waybill.Order_ID == d.Order_ID
                            select d.Bookings).FirstOrDefault();

                if (date.Book_DeliveryDate != DateTime.Now.Date)
                {
                    var book = db.Bookings.Find(date.Book_ID);

                    book.Book_DeliveryDate = DateTime.Now.Date;

                    db.Entry(book).State = EntityState.Modified;
                   



                    var emails = db.Clients.Where(c => c.Client_ID == book.Client_ID).Select(c => c.Client_Email).FirstOrDefault();

                    var name = db.Clients.Where(c => c.Client_ID == book.Client_ID).Select(c => c.Client_Name).FirstOrDefault();
                    var lastName = db.Clients.Where(c => c.Client_ID == book.Client_ID).Select(c => c.Client_Surname).FirstOrDefault();


                    // send  emails to notify client of early delivery
                    #region MailBooking

                    string body = "Dear " + name + " " + lastName + "<br/>"
                        + "<br/>"
                        + "<h2>Your package with tracking No: " + waybill.Track_ID + " has been rescheduled to dispatch today</h2>"

                        + "<br/>"
                        + "<br/>" +


                        "<br/>" +
                        "<br/>" +

                        "If you have any queries please reply to this email" +
                         "<br/>" +
                        "<br/>" +
                        "Regards," +
                        "<br/>" +
                        "Messenger Kings Courier Management";

                    GmailClass gm = new GmailClass();
                    gm.Gmail("Messanger King ~ Your Package Has Been Dispatched", body, emails);

                    #endregion
                }

                db.Entry(waybill).State = EntityState.Modified;
                db.Entry(tracking).State = EntityState.Modified;
                db.SaveChanges();

               TempData["Success"] = "You have successfully updated the status of this package, please scan a new package or Type in a Tracking number";
                return RedirectToAction("Store");
            }



            ViewBag.Status_ID = new SelectList(db.Status, "Status_ID", "Storage_Status", waybill.Order_ID);
            ViewBag.Order_ID = new SelectList(db.Orders, "Order_ID", "Driver_ID", waybill.Order_ID);
            return View(waybill);
        }

        public ActionResult Store()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Store(Tracking id)
        {
                if (id.Track_ID == null)
                {

                ViewBag.Error = "Tracking number does not exist";
                return View();

                }
                
                Tracking tracking = db.Trackings.Find(id.Track_ID);
                if (tracking == null)
                {
                ViewBag.Error = "Tracking number not found";
                return View();
                }

            var status = (from d in db.Trackings
                         where d.Track_ID == id.Track_ID
                         select d.Track_Message).FirstOrDefault();

            if (status == "Order has been Picked up")
            {
               
                return RedirectToAction("ManagePackage","Waybills", new { id = id.Track_ID });
            }
            else if (status == "Order has arrived at Warehouse")
            {

                return RedirectToAction("ManagePackageDispatch","Waybills", new { id = id.Track_ID });
            }
          
            ViewBag.Error = "Package tracking error. Move on to next package";

            return View();
           
        }

        public ActionResult SuccessfulStore()
        {
            return View();
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
