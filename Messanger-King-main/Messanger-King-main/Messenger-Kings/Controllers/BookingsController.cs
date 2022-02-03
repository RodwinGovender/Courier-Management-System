using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Messenger_Kings.Models;
using Microsoft.AspNet.Identity;
using PayFast;
using PayFast.AspNet;

namespace Messenger_Kings.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public BookingsController()
        {
            this.payFastSettings = new PayFastSettings();
            this.payFastSettings.MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            this.payFastSettings.MerchantKey = ConfigurationManager.AppSettings["MerchantKey"];
            this.payFastSettings.PassPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            this.payFastSettings.ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"];
            this.payFastSettings.ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"];
            this.payFastSettings.ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            this.payFastSettings.CancelUrl = ConfigurationManager.AppSettings["CancelUrl"];
            this.payFastSettings.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];
        }

        public ActionResult Index(string searchString)
        {
            var uid = User.Identity.GetUserId();
            var cam = (from i in db.Contracts
                       where i.Bookings.Client_ID == uid && i.Co_Status == "Unpaid"
                       select i.Co_Amount)
           .DefaultIfEmpty(0)
           .Sum();







            ViewBag.Contract = cam;
            if (!String.IsNullOrEmpty(searchString))
            {
                var book = from s in db.Bookings
                           select s;
                book = book.Where(s =>
               s.Book_RecipientName.ToUpper().Contains(searchString.ToUpper()) || s.Book_PickupDate.ToString().ToUpper().Contains(searchString.ToUpper()) || s.Book_RecipientSurname.ToString().ToUpper().Contains(searchString.ToUpper()) || s.Quote.Quote_PickupAddress.ToString().ToUpper().Contains(searchString.ToUpper())).Where(b => b.Client_ID == uid);
                return View(book.ToList());
            }



           

            
            var bookings = db.Bookings.Include(b => b.Client).Include(b => b.Quote).Where(b => b.Client_ID == uid).OrderByDescending(b => b.Book_PickupDate).ToList();
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            if (booking.Coupon_used != null)
            {


                ViewBag.NoCoupon = "Applied Successfully";

            }


            return View(booking);


        }

        [HttpPost]
        public ActionResult Details(Booking booking)
        {



            if (booking.Coupon_used != "")
            {

                Booking bookings = db.Bookings.Find(booking.Book_ID);

                if (bookings == null)
                {

                    return RedirectToAction("Create");

                }

                var uid = User.Identity.GetUserId();

                var coupon = (from d in db.Coupons
                              where d.Coupon_Code == booking.Coupon_used && d.Client_ID == uid
                              select d).FirstOrDefault();



                if (coupon == null)
                {

                    ViewBag.NoCoupon = "Coupon does not exist. Please Ensure It Is Correct";
                    return View(bookings);
                }
                else if (coupon.Coupon_Status == "Used")
                {
                    ViewBag.NoCoupon = "Coupon Is already used. Please try another coupon";
                    return View(bookings);
                }


                ViewBag.NoCoupon = "Applied Successfully";


                TempData["OldValue"] = bookings.Book_TotalCost;
                TempData["Value"] = coupon.Coupon_Value;

                bookings.Coupon_used = coupon.Coupon_Code;

                bookings.Book_TotalCost = bookings.Book_TotalCost - coupon.Coupon_Value;

                if (booking.Book_TotalCost < 0)
                {
                    booking.Book_TotalCost = 0;

                }

                coupon.Coupon_Status = "Used";

                db.Entry(coupon).State = EntityState.Modified;
                db.Entry(bookings).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", bookings.Book_ID);
            }
            return RedirectToAction("Details");

        }


        // GET: Bookings/Create
        public ActionResult Create(int? id)
        {
            Booking booking = new Booking();
            booking.Quote_ID = (int)id;
            booking.Book_PickupDate = DateTime.Today;
            booking.Book_DeliveryDate = DateTime.Today.AddDays(1);
            var uid = User.Identity.GetUserId();
            booking.Client_ID = uid;
            ViewBag.Client_ID = new SelectList(db.Clients, "Client_ID", "Client_IDNo");
            ViewBag.Quote_ID = new SelectList(db.Quotes, "Quote_ID", "Quote_PickupAddress");
            return View(booking);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Book_ID,Book_PickupDate,Book_DeliveryDate,Book_RecipientName,Book_RecipientSurname,Book_RecipientNumber,Book_DeliveryNote,Book_TotalCost,BookStatus,Quote_ID,Client_ID")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                var cost = (from cl in db.Quotes
                            where cl.Quote_ID == booking.Quote_ID
                            select cl.Quote_Cost).SingleOrDefault();
                booking.Book_TotalCost = cost;
                if (booking.Book_DeliveryNote == null || booking.Book_DeliveryNote == "")
                {
                    booking.Book_DeliveryNote = "None";
                }

                var uid = User.Identity.GetUserId();
                var getCat = (from c in db.Clients
                              where c.Client_ID == uid
                              select c.ClientCategory.ClientCat_Type).FirstOrDefault();

                //Validate dates selected 
                if (booking.Book_PickupDate > booking.Book_DeliveryDate || booking.Book_PickupDate == booking.Book_DeliveryDate || booking.Book_PickupDate < DateTime.Today || booking.Book_DeliveryDate == DateTime.Today || booking.Book_DeliveryDate < DateTime.Today)
                {
                    ViewBag.ErrorDate = "Please check if your dates are correct and try again.";
                    return View(booking);
                }
                db.Bookings.Add(booking);
                db.SaveChanges();

                var emails = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Email).FirstOrDefault();
                var id = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == booking.Book_ID).Select(c => c.Book_ID).FirstOrDefault();
                var deliveryDate = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == booking.Book_ID).Select(c => c.Book_DeliveryDate).FirstOrDefault();
                var totcost = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == booking.Book_ID).Select(c => c.Book_TotalCost).FirstOrDefault();
                // fetch current user details
                var name = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Name).FirstOrDefault();
                var lastName = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Surname).FirstOrDefault();
                var IdNo = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_IDNo).FirstOrDefault();
                var contact = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Cellnumber).FirstOrDefault();
                var addr = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_Address).FirstOrDefault();
                var Tell = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == booking.Book_ID).Select(c => c.Client.Client_IDNo).FirstOrDefault();
                var description = db.Bookings.Where(c => c.Client_ID == booking.Client_ID && c.Book_ID == booking.Book_ID).Select(c => c.Quote.Quote_Description).FirstOrDefault();

                var pickup = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == booking.Book_ID).Select(c => c.Book_PickupDate).FirstOrDefault();
                // check if the user details are complete before proceding with booking
                if (name == null || lastName == null || IdNo == null || contact == null || addr == null || Tell == null)
                {
                    //ViewBag.ErrorData = $"Your profile may be incomplete check if your details are complete.";
                    ////Task.WaitAll(Task.Delay(1000));
                    db.Bookings.Find(booking.Book_ID);
                    db.Bookings.Remove(booking);
                    return RedirectToAction("Edit", "Clients", new { id = uid });
                }

                //create instance for emails
                // Email objmail = new Email();
                // objmail.SendConfirmation(emails, name, id, deliveryDate, (double)totcost);
                

                return RedirectToAction("Details", new { id = booking.Book_ID });


            }

            ViewBag.Client_ID = new SelectList(db.Clients, "Client_ID", "Client_IDNo", booking.Client_ID);
            ViewBag.Quote_ID = new SelectList(db.Quotes, "Quote_ID", "Quote_PickupAddress", booking.Quote_ID);
            return View(booking);
        }

        // GET: Bookings/Edit/
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.Client_ID = new SelectList(db.Clients, "Client_ID", "Client_IDNo", booking.Client_ID);
            ViewBag.Quote_ID = new SelectList(db.Quotes, "Quote_ID", "Quote_PickupAddress", booking.Quote_ID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Book_ID,Book_PickupDate,Book_DeliveryDate,Book_RecipientName,Book_RecipientSurname,Book_RecipientNumber,Book_DeliveryNote,Book_TotalCost,BookStatus,Quote_ID,Client_ID")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                var cost = (from cl in db.Quotes
                            where cl.Quote_ID == booking.Quote_ID
                            select cl.Quote_Cost).SingleOrDefault();

                booking.Book_TotalCost = cost;

                var uid = User.Identity.GetUserId();
                var getCat = (from c in db.Clients
                              where c.Client_ID == uid
                              select c.ClientCategory.ClientCat_Type).FirstOrDefault();

                //Validate dates selected 
                if (booking.Book_PickupDate > booking.Book_DeliveryDate || booking.Book_PickupDate == booking.Book_DeliveryDate || booking.Book_PickupDate < DateTime.Today || booking.Book_DeliveryDate == DateTime.Today || booking.Book_DeliveryDate < DateTime.Today)
                {
                    ViewBag.ErrorDate = "Please check if your dates are correct and try again. (Delivery date must be after Pickup date)";
                    return View(booking);
                }
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                //create instance for emails
              
                // send  emails and redirect to 
                //objmail.SendConfirmation(emails, name, id, deliveryDate, (double)totcost);
                return RedirectToAction("Details", new { id = booking.Book_ID });

            }
            ViewBag.Client_ID = new SelectList(db.Clients, "Client_ID", "Client_IDNo", booking.Client_ID);
            ViewBag.Quote_ID = new SelectList(db.Quotes, "Quote_ID", "Quote_PickupAddress", booking.Quote_ID);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public ActionResult Validate(int id)
        {
            // Email objmail = new Email();
            var uid = User.Identity.GetUserId();
            var bookingt = (from h in db.Bookings
                            where h.Book_ID == id
                            select h).FirstOrDefault();
            var getCat = (from c in db.Bookings
                          where c.Book_ID == id
                          select c.Client.ClientCategory.ClientCat_Type).FirstOrDefault();
            var Qid = (from c in db.Bookings
                       where c.Book_ID == id
                       select c.Quote_ID).FirstOrDefault();
            var getTempCat = (from c in db.Quotes
                              where c.Quote_ID == Qid
                              select c.ClientCategory.ClientCat_Type).FirstOrDefault();

            var emails = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == bookingt.Book_ID).Select(c => c.Client.Client_Email).FirstOrDefault();
            var idy = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == bookingt.Book_ID).Select(c => c.Book_ID).FirstOrDefault();
            var deliveryDate = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == bookingt.Book_ID).Select(c => c.Book_DeliveryDate).FirstOrDefault();
            var totcost = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == bookingt.Book_ID).Select(c => c.Book_TotalCost).FirstOrDefault();
            // fetch current user details
            var name = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == bookingt.Book_ID).Select(c => c.Client.Client_Name).FirstOrDefault();
            var lastName = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == bookingt.Book_ID).Select(c => c.Client.Client_Surname).FirstOrDefault();
            var IdNo = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == bookingt.Book_ID).Select(c => c.Client.Client_IDNo).FirstOrDefault();
            var contact = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == bookingt.Book_ID).Select(c => c.Client.Client_Cellnumber).FirstOrDefault();
            var addr = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == bookingt.Book_ID).Select(c => c.Client.Client_Address).FirstOrDefault();
            var Tell = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == bookingt.Book_ID).Select(c => c.Client.Client_IDNo).FirstOrDefault();
            var description = db.Bookings.Where(c => c.Client_ID == bookingt.Client_ID && c.Book_ID == bookingt.Book_ID).Select(c => c.Quote.Quote_Description).FirstOrDefault();
            var pickup = db.Bookings.Where(c => c.Client_ID == uid && c.Book_ID == bookingt.Book_ID).Select(c => c.Book_PickupDate).FirstOrDefault();

            if (getCat.ToUpper() == "Pay as you go".ToUpper() && getTempCat.ToUpper() == "Pay as you go".ToUpper())
            {
                // send  emails and redirect to 
                #region MailBooking

                string body = "Dear " + name + " " + lastName + "<br/>"
                    + "<br/>"
                    + "<h2>Congratulations You have successfully booked for a courier As a Pay as You go Customer</h2>"

                    + "<br/>"
                    + "<br/>"
                    + "<h3>Your Booking Details:</h3>"


                    + "ID Number: " + IdNo + "<br/>"
                    + "Contact Number: " + contact + "<br/>" + "<br/>" + "<br/>"
                    + "Your Item's Description: " + description + "<br/>"
                    + "The Pick-up Date Is: " + pickup + "<br/>"
                    + "The Delivery Date Is: " + deliveryDate.Date
                    + "<br/>" + "The Total Cost: R" + totcost +
                    "<br/>" +
                    "<h3>You will be notified as soon as your order is approved by one of our admins</h3>" +
                    "<br/>" +
                    "<br/>" +
                    "<br/>" +

                    "Regards," +
                    "<br/>" +
                    "Messenger Kings Courier Management";

                GmailClass gm = new GmailClass();
                gm.Gmail("Messanger King ~ Booking Confirmation", body, emails);
                #endregion MailBooking
                // objmail.SendConfirmation(emails, name, id, deliveryDate, (double)totcost);

                var booking = db.Bookings.Find(id);

                if (booking.Book_TotalCost == 0)
                {



                    var spin = (from b in db.Spins
                                where b.Client_ID == uid
                                select b).FirstOrDefault();




                    spin.No_Spins += 1;

                    db.Entry(spin).State = EntityState.Modified;
                    booking.paymentstatus = true;
                    db.Entry(booking).State = EntityState.Modified;
                    db.SaveChanges();
                    // objmail.SendConfirmation(emails, name, id, deliveryDate, (double)totcost);
                    return RedirectToAction("SuccessfulBooking", new { id = id });
                }
                
                return RedirectToAction("OnceOff", new { id = id });


            }
            else if (getCat.ToUpper() == "On contract".ToUpper() && getTempCat.ToUpper() == "Pay as you go".ToUpper())
            {
                #region MailBooking

                string body = "Dear " + name + " " + lastName + "<br/>"
                    + "<br/>"
                    + "<h2>Congratulations You have successfully booked for a courier As a Pay as You go Customer</h2>"

                    + "<br/>"
                    + "<br/>"
                    + "<h3>Your Booking Details:</h3>"


                    + "ID Number: " + IdNo + "<br/>"
                    + "Contact Number: " + contact + "<br/>" + "<br/>" + "<br/>"
                    + "Your Item's Description: " + description + "<br/>"
                    + "The Pick-up Date Is: " + pickup + "<br/>"
                    + "The Delivery Date Is: " + deliveryDate.Date
                    + "<br/>" + "The Total Cost: R" + totcost +
                    "<br/>" +
                    "<h3>You will be notified as soon as your order is approved by one of our admins</h3>" +
                    "<br/>" +
                    "<br/>" +
                    "<br/>" +

                    "Regards," +
                    "<br/>" +
                    "Messenger Kings Courier Management";

                GmailClass gm = new GmailClass();
                gm.Gmail("Messanger King ~ Booking Confirmation", body, emails);
                #endregion MailBooking
                // objmail.SendConfirmation(emails, name, id, deliveryDate, (double)totcost);

                var booking = db.Bookings.Find(id);

                if (booking.Book_TotalCost == 0)
                {



                    var spin = (from b in db.Spins
                                where b.Client_ID == uid
                                select b).FirstOrDefault();




                    spin.No_Spins += 1;

                    db.Entry(spin).State = EntityState.Modified;
                    booking.paymentstatus = true;
                    db.Entry(booking).State = EntityState.Modified;
                    db.SaveChanges();
                    // objmail.SendConfirmation(emails, name, id, deliveryDate, (double)totcost);
                    return RedirectToAction("SuccessfulBooking", new { id = id });
                }

                return RedirectToAction("OnceOff", new { id = id });

            }
            else
            {
                var booking = db.Bookings.Find(id);
                #region MailBooking

                string body = "Dear " + name + " " + lastName + "<br/>"
                    + "<br/>"
                    + "<h2>Congratulations You have successfully booked for a courier As a Contract Customer</h2>"

                    + "<br/>"
                    + "<br/>"
                    + "<h3>Your Booking Details:</h3>"


                    + "ID Number: " + IdNo + "<br/>"
                    + "Contact Number: " + contact + "<br/>" + "<br/>" + "<br/>"
                    + "Your Item's Description: " + description + "<br/>"
                    + "The Pick-up Date Is: " + pickup + "<br/>"
                    + "The Delivery Date Is: " + deliveryDate.Date
                    + "<br/>" + "The Total Cost: R" + totcost +
                    "<br/>" +
                    "<h3>You will be notified as soon as your order is approved by one of our admins</h3>" +
                    "<br/>" +
                    "<br/>" +
                    "<br/>" +

                    "Regards," +
                    "<br/>" +
                    "Messenger Kings Courier Management";

                GmailClass gm = new GmailClass();
                gm.Gmail("Messanger King ~ Booking Confirmation", body, emails);
                #endregion MailBooking
                var spin = (from b in db.Spins
                            where b.Client_ID == uid
                            select b).FirstOrDefault();



                spin.No_Spins += 1;

                Contract co = new Contract();

                co.Co_Amount = booking.Book_TotalCost;
                co.Co_Status = "Unpaid";
                co.Book_ID = booking.Book_ID;

                db.Contracts.Add(co);
                db.Entry(spin).State = EntityState.Modified;
                booking.paymentstatus = true;
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                // objmail.SendConfirmation(emails, name, id, deliveryDate, (double)totcost);
                return RedirectToAction("SuccessfulBooking", new { id = id });
            }
        }


        public ActionResult Paynow()
        {
            return RedirectToAction("Success");
        }
        public ActionResult Success()
        {
            //var uid = User.Identity.GetUserId();
            //var id = (from c in db.Bookings
            //         where c.Client_ID == uid
            //         select c).LastOrDefault();


            //id.paymentstatus = true;

            //db.Entry(id).State = EntityState.Modified;
            //db.SaveChanges();

            //Add email for pay as you go
            return RedirectToAction("Index", "Home");
        }


        public ActionResult SuccessfulBooking(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }


            return View(booking);
            //Add email for contract
        }


        public ActionResult Error()
        {
            return View("Index");
        }

        #region Fields

        private readonly PayFastSettings payFastSettings;

        #endregion Fields

        #region Constructor


        #endregion Constructor

        #region Methods



        public ActionResult Recurring()
        {
            var recurringRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            // Merchant Details
            recurringRequest.merchant_id = this.payFastSettings.MerchantId;
            recurringRequest.merchant_key = this.payFastSettings.MerchantKey;
            recurringRequest.return_url = this.payFastSettings.ReturnUrl;
            recurringRequest.cancel_url = this.payFastSettings.CancelUrl;
            recurringRequest.notify_url = this.payFastSettings.NotifyUrl;
            // Buyer Details
            recurringRequest.email_address = "nkosi@finalstride.com";
            // Transaction Details
            recurringRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            recurringRequest.amount = 20;
            recurringRequest.item_name = "Recurring Option";
            recurringRequest.item_description = "Some details about the recurring option";
            // Transaction Options
            recurringRequest.email_confirmation = true;
            recurringRequest.confirmation_address = "drnendwandwe@gmail.com";
            // Recurring Billing Details
            recurringRequest.subscription_type = SubscriptionType.Subscription;
            recurringRequest.billing_date = DateTime.Now;
            recurringRequest.recurring_amount = 20;
            recurringRequest.frequency = BillingFrequency.Monthly;
            recurringRequest.cycles = 0;
            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{recurringRequest.ToString()}";
            return Redirect(redirectUrl);
        }




        public ActionResult OnceOff(int? id)
        {
            Booking booking = db.Bookings.Find(id);

            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;
            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            double amount = (double)booking.Book_TotalCost;

            //var products = db.Items.Select(x => x.Item_Name).ToList();
            // Transaction Details
            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = amount;
            onceOffRequest.item_name = "Your Order number is: " + id;
            onceOffRequest.item_description = "You are now paying your rental fee";
            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

            //remove when deployed
            booking.paymentstatus = true;


            var uid = User.Identity.GetUserId();
            var spin = (from b in db.Spins
                        where b.Client_ID == uid
                        select b).FirstOrDefault();




            spin.No_Spins += 1;

            db.Entry(spin).State = EntityState.Modified;

            db.Entry(booking).State = EntityState.Modified;

            db.SaveChanges();


            return Redirect(redirectUrl);
        }



        public ActionResult ContractPayment()
        {
            var uid = User.Identity.GetUserId();

            var camount = (from i in db.Contracts
                           where i.Bookings.Client_ID == uid && i.Co_Status == "Unpaid"
                           select i.Co_Amount).Sum();

            var unpaidC = (from i in db.Contracts
                           where i.Bookings.Client_ID == uid && i.Co_Status == "Unpaid"
                           select i);

            foreach (var i in unpaidC)
            {
                i.Co_Status = "Paid";
                db.Entry(i).State = EntityState.Modified;

            }

            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;
            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            double amount = (double)camount;

            //var products = db.Items.Select(x => x.Item_Name).ToList();
            // Transaction Details
            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = amount;
            onceOffRequest.item_name = "Contract Payment";
            onceOffRequest.item_description = "You are now paying your Contract Amount";
            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

            //remove when deployed
           

           

         

           

            db.SaveChanges();


            return Redirect(redirectUrl);
        }









        public ActionResult AdHoc()
        {
            var adHocRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            adHocRequest.merchant_id = this.payFastSettings.MerchantId;
            adHocRequest.merchant_key = this.payFastSettings.MerchantKey;
            adHocRequest.return_url = this.payFastSettings.ReturnUrl;
            adHocRequest.cancel_url = this.payFastSettings.CancelUrl;
            adHocRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            adHocRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            adHocRequest.m_payment_id = "";
            adHocRequest.amount = 70;
            adHocRequest.item_name = "Adhoc Agreement";
            adHocRequest.item_description = "Some details about the adhoc agreement";

            // Transaction Options
            adHocRequest.email_confirmation = true;
            adHocRequest.confirmation_address = "sbtu01@payfast.co.za";

            // Recurring Billing Details
            adHocRequest.subscription_type = SubscriptionType.AdHoc;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{adHocRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        public ActionResult Return()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Notify([ModelBinder(typeof(PayFastNotifyModelBinder))] PayFastNotify payFastNotifyViewModel)
        {
            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            System.Diagnostics.Debug.WriteLine($"Signature Validation Result: {isValid}");

            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases
            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, IPAddress.Parse(this.HttpContext.Request.UserHostAddress));

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            System.Diagnostics.Debug.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

            System.Diagnostics.Debug.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

            // Currently seems that the data validation only works for successful payments
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = await payfastValidator.ValidateData();

                System.Diagnostics.Debug.WriteLine($"Data Validation Result: {dataValidationResult}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                System.Diagnostics.Debug.WriteLine($"Subscription was cancelled");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        

        #endregion Methods
    }
}
