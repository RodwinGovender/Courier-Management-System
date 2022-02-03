using Messenger_Kings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Messenger_Kings.Controllers
{
    public class MapsController: Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? id)
        {
            if (id == null)
           {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order orders = db.Orders.Find(id);
            var trackingStatus = db.Trackings.Include(p => p.Order).Where(p => p.Order_ID == orders.Order_ID).FirstOrDefault();
            if (trackingStatus.Track_Message == "Out for Pickup")
            {


                ViewBag.Status = "Pickup";

            }
            else if(trackingStatus.Track_Message == "Package has been dispatched from Warehouse")
            {


                ViewBag.Status = "Dropoff";

            }
            else
            {
                ViewBag.Status = trackingStatus.Track_Message;

            }
            if (orders == null)
            {
                return HttpNotFound();
            }

            ViewBag.TrackID = trackingStatus.Track_ID;

            ViewBag.tracking = trackingStatus;

            return View(orders);
        

        }
        public ActionResult Details()
        {
            return View();
        }
        public ActionResult Animate()
        {
            return View();
        }
        public ActionResult Icon()
        {
            return View();
        }
        public ActionResult Steet()
        {
            return View();
        }
        public ActionResult ModeTravel()
        {
            return View();
        }
        public ActionResult Traffic()
        {
            return View();
        }
        public ActionResult RouteColor()
        {
            return View();
        }
        public ActionResult DistanceCalculation()
        {
            return View();
        }
    }
}