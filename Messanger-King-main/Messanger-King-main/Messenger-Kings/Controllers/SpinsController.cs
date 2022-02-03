using Messenger_Kings.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Messenger_Kings.Controllers
{
    public class SpinsController : Controller
    {
        // GET: Spins
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Win()
        {
            ModelState.Clear();
            var uid = User.Identity.GetUserId();
            Spins spin = (from b in db.Spins
                          where b.Client_ID == uid
                          select b).FirstOrDefault();

            var list = (from c in db.Coupons
                        where c.Client_ID == uid && c.Coupon_Status != "Used"
                        select c).ToList();

            ViewBag.SpinsLeft = spin.No_Spins;

            return View(list);
        }

        [HttpPost]
        public ActionResult Win(Coupon coupon)
        {
           
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[4];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            var uid = User.Identity.GetUserId();
            coupon.Coupon_Code = coupon.Coupon_Value + "RandOFF" + finalString;
            coupon.Client_ID = User.Identity.GetUserId();


            var spin = (from b in db.Spins
                        where b.Client_ID == uid
                        select b).FirstOrDefault();

            spin.No_Spins -= 1;

            ViewBag.SpinsLeft = spin.No_Spins;
            ViewBag.CouponCode = coupon.Coupon_Code;
            TempData["Coupons"] = "TEST";
            db.Entry(spin).State = EntityState.Modified;
            db.Coupons.Add(coupon);
            db.SaveChanges();


            var emails = db.Clients.Where(c => c.Client_ID == uid).Select(c => c.Client_Email).FirstOrDefault();

            var name = db.Clients.Where(c => c.Client_ID == uid).Select(c => c.Client_Name).FirstOrDefault();
            var lastName = db.Clients.Where(c => c.Client_ID == uid).Select(c => c.Client_Surname).FirstOrDefault();


            // send  emails and redirect to 
            #region MailBooking

            string body = "Dear " + name + " " + lastName + "<br/>"
                + "<br/>"
                + "<h2>Congratulations You Have Won A R" + coupon.Coupon_Value + " Coupon</h2>"

                + "<br/>"
                + "<br/>"
                + "<h3>Your Coupon Code Is: " +
                coupon.Coupon_Code +
                "</h3>" +


                "<br/>" +
                "<br/>" +

                "You may use it at checkout to recieve a discount of the specified value." +
                 "<br/>" +
                "<br/>" +
                "Regards," +
                "<br/>" +
                "Messenger Kings Courier Management";

            GmailClass gm = new GmailClass();
            gm.Gmail("Messanger King ~ You Won a Coupon!", body, emails);
            #endregion



            return View(coupon);
        }
    }
}