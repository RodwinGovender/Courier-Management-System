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
using Rotativa;

namespace Messenger_Kings.Controllers
{
   
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Bookings).Include(o => o.Driver);
            return View(orders.ToList());
        }

        
        public ActionResult OrderList()
        {
            var orders = db.Bookings.ToList().Where(a=>a.BookStatus == false && a.paymentstatus == true);

         

            return View(orders.ToList());
        }



        //public ActionResult DriverList()
        //{
        //    var orders = db.Bookings.ToList().Where(a => a.BookStatus == true && a.paymentstatus == true);

        //    return View(orders.ToList());
        //}

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }


      
        public ActionResult Decline(int id)
        {
            

            var booking = (from b in db.Bookings
                           where b.Book_ID == id
                           select b).SingleOrDefault();
          
            booking.paymentstatus = false;

            booking.BookDecline = true;
       
            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();

            #region MailDecline

            var emails = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Email).FirstOrDefault();
            var name = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Name).FirstOrDefault();
            var description = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Quote.Quote_Description).FirstOrDefault();
            var deliveryDate = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Book_DeliveryDate).FirstOrDefault();
            var totcost = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Book_TotalCost).FirstOrDefault();
            // fetch current user details
            var lastName = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Surname).FirstOrDefault();
            var IdNo = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_IDNo).FirstOrDefault();
            var contact = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Cellnumber).FirstOrDefault();
            var addr = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Address).FirstOrDefault();
            var Tell = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_IDNo).FirstOrDefault();

            var pickup = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Book_PickupDate).FirstOrDefault();


            string body = "Dear " + name + " " + lastName + "<br/>"
                    + "<br/>"
                    + "<h2>Your order with description name " + description + " has been declined for delivery by Messenger King Courier...</h2> "
                    + "<br/>"
                    + "<br/>"
                    + "<br/><b>" + "The Total Cost (R" + totcost + ") Will Be Refunded To Your Payment Method.</b>" +

                    "<br/>" +
                    "<br/>" +
                    " If you wish to create another booking please visit our website " +
                    "<br/>" +
                    "<br/>" +


                    "Regards, " +
                    "<br/>" +
                    "Messenger Kings Courier Management";

                GmailClass gm = new GmailClass();
                gm.Gmail("Messanger King ~ Order Declined", body, emails);
                #endregion MailDecline                   

            return RedirectToAction("OrderList");
        }


        // GET: Orders/Create

        public ActionResult Create(int id)
        {


            var order = new Order();

            order.Book_ID = id;

            ViewBag.Book_ID = new SelectList(db.Bookings, "Book_ID", "Book_RecipientName");
            ViewBag.Driver_ID = new SelectList(db.Drivers, "Driver_ID", "Driver_Name");
            return View(order);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Order_ID,Order_DateTime,Order_DeliveryDate,Book_ID,Driver_ID")] Order order)
        {
            if (ModelState.IsValid)
            {

                
                var booking = (from b in db.Bookings
                               where b.Book_ID == order.Book_ID
                               select b).SingleOrDefault();

                booking.BookStatus = true;


                var orderDate = (from f in db.Bookings
                                 where f.Book_ID == order.Book_ID
                                 select f.Book_PickupDate).FirstOrDefault();

                order.Order_DateTime = DateTime.Now;
                

                 Tracking tracking = new Tracking();
            //Booking book = db.Bookings.Find(order);

            tracking.Track_ID = "MK-" + booking.Book_ID + "87448972";
            tracking.Order_Status = "Your order has been approved!";

                tracking.Track_Message = "The Order Has Been Approved!";


                #region MailApproved

                var emails = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Email).FirstOrDefault();
                var name = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Name).FirstOrDefault();
                var description = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Quote.Quote_Description).FirstOrDefault();
                var deliveryDate = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Book_DeliveryDate).FirstOrDefault();
                var totcost = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Book_TotalCost).FirstOrDefault();
                // fetch current user details
                var lastName = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Surname).FirstOrDefault();
                var IdNo = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_IDNo).FirstOrDefault();
                var contact = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Cellnumber).FirstOrDefault();
                var addr = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Address).FirstOrDefault();
                var Tell = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_IDNo).FirstOrDefault();

                var pickup = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Book_PickupDate).FirstOrDefault();





                string body = "Dear " + name + " " + lastName + "<br/>"
                    + "<br/>"
                    + "<h3>Your order with description name " + description + " has been approved for delivery by Messenger King Courier...</h3> "
                    + "<br/>" +
                     "<h3>You Are Now Able To Track Your Package Using The Following Tracking Number : <u>" + tracking.Track_ID + "</u></h3>" +

                    "<br/>" +
                    "<br/>" +
                    " If you wish to create another booking please visit our website " +
                    "<br/>" +
                    "<br/>" +

                    "Regards, " +
                    "<br/>" +
                    "Messenger Kings Courier Management";

                GmailClass gm = new GmailClass();
                gm.Gmail("Messanger King ~ Order Approved!", body, emails);
                #endregion MailApproved





                db.Trackings.Add(tracking);
                
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Book_ID = new SelectList(db.Bookings, "Book_ID", "Book_RecipientName", order.Book_ID);
            ViewBag.Driver_ID = new SelectList(db.Drivers, "Driver_ID", "Driver_Name", order.Driver_ID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Book_ID = new SelectList(db.Bookings, "Book_ID", "Book_RecipientName", order.Book_ID);
            ViewBag.Driver_ID = new SelectList(db.Drivers, "Driver_ID", "Driver_Name", order.Driver_ID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Order_ID,Order_DateTime,Order_DeliveryDate,Book_ID,Driver_ID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Book_ID = new SelectList(db.Bookings, "Book_ID", "Book_RecipientName", order.Book_ID);
            ViewBag.Driver_ID = new SelectList(db.Drivers, "Driver_ID", "Driver_Name", order.Driver_ID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }


        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ViewPDF(int id)
        {


            var report = new Rotativa.ActionAsPdf("Invoice", new { id = id }) { FileName = "Invoice.pdf" };
            return report; 

            //return new ViewAsPdf("Invoice", new { id = id });
        }

        public ActionResult Invoice(int? id)
        {

            var orderId = (from b in db.Orders
                           where b.Book_ID == id
                           select b.Order_ID).SingleOrDefault();

            //if (orderId == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Order order = db.Orders.Find(orderId);
            if (order == null)
            {
                return HttpNotFound();
            }


            return View(order);
        }

        public ActionResult DeliveryList()
        {
            var uid = User.Identity.GetUserId();


            var orders = db.Trackings.Where(a => a.Order.Driver.Driver_ID == uid && ( a.Track_Message == "Out for Pickup" || a.Track_Message == "Package has been dispatched from Warehouse"));

            return View(orders.ToList());
      
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
