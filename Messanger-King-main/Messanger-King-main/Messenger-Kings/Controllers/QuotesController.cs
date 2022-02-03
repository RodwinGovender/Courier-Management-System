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
    public class QuotesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Quotes
        public ActionResult Index()
        {
            var quotes = db.Quotes.Include(q => q.ClientCategory);
            return View(quotes.ToList());
        }

        // GET: Quotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.Quotes.Find(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // GET: Quotes/Create
        public ActionResult Create()
        {
            ViewBag.ClientCat_ID = new SelectList(db.ClientCategories, "ClientCat_ID", "ClientCat_Type");

            return View();
        }

        // POST: Quotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Quote_ID,Quote_Date,Quote_PickupAddress,Quote_DeliveryAddress,Quote_Distance,Quote_Description,Quote_Cost,Item_Quantity,Quote_length,Quote_Height,Quote_Width,Quote_Weight,ClientCat_ID")] Quote quote)
        {
            if (ModelState.IsValid)
            {

                quote.Quote_Date = System.DateTime.Now;


                var rateId = db.Rates.Where(rat => rat.ClientCat_ID == db.ClientCategories.Where(cat => cat.ClientCat_ID == quote.ClientCat_ID).Select(c => c.ClientCat_ID).FirstOrDefault()).Select(r => r.Rate_ID).FirstOrDefault();
                var cost = ((db.Rates.Where(cmtr => cmtr.ClientCat_ID == rateId).Select(rpcm => rpcm.Rate_PerCM).FirstOrDefault() *
                     (decimal)(quote.Quote_length + quote.Quote_Height + quote.Quote_Width) +
                     (db.Rates.Where(klg => klg.ClientCat_ID == rateId).Select(kgs => kgs.Rate_PerKG).FirstOrDefault() *
                     (decimal)quote.Quote_Width)) *
                     (decimal)quote.Item_Quantity) +
                     (db.Rates.Where(klmt => klmt.ClientCat_ID == rateId).Select(klm => klm.Rate_PerKM).FirstOrDefault()
                     * (decimal)quote.Quote_Distance);

                cost += db.Rates.Where(bsc => bsc.ClientCat_ID == rateId).Select(bsc => bsc.Base_Cost).FirstOrDefault();
                quote.Quote_Cost = cost;
                db.Quotes.Add(quote);
                db.SaveChanges();
                return RedirectToAction("Details", new
                {
                    id = quote.Quote_ID
                });
            }

            ViewBag.ClientCat_ID = new SelectList(db.ClientCategories, "ClientCat_ID", "ClientCat_Type", quote.ClientCat_ID);
            return View(quote);
        }


        public JsonResult Getdistance(Quote quote)
        {
          
            return Json(quote.Quote_Distance, JsonRequestBehavior.AllowGet);
        }


        // GET: Quotes/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.Quotes.Find(id);
          

            if (quote == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientCat_ID = new SelectList(db.ClientCategories, "ClientCat_ID", "ClientCat_Type", quote.ClientCat_ID);
            return View(quote);
        }

        // POST: Quotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Quote_ID,Quote_Date,Quote_PickupAddress,Quote_DeliveryAddress,Quote_Distance,Quote_Description,Quote_Cost,Item_Quantity,Quote_length,Quote_Height,Quote_Width,Quote_Weight,ClientCat_ID")] Quote quote)
        {
            if (ModelState.IsValid)
            {

                quote.Quote_Date = System.DateTime.Now;


                var rateId = db.Rates.Where(rat => rat.ClientCat_ID == db.ClientCategories.Where(cat => cat.ClientCat_ID == quote.ClientCat_ID).Select(c => c.ClientCat_ID).FirstOrDefault()).Select(r => r.Rate_ID).FirstOrDefault();
                var cost = ((db.Rates.Where(cmtr => cmtr.ClientCat_ID == rateId).Select(rpcm => rpcm.Rate_PerCM).FirstOrDefault() *
                     (decimal)(quote.Quote_length + quote.Quote_Height + quote.Quote_Width) +
                     (db.Rates.Where(klg => klg.ClientCat_ID == rateId).Select(kgs => kgs.Rate_PerKG).FirstOrDefault() *
                     (decimal)quote.Quote_Width)) *
                     (decimal)quote.Item_Quantity) +
                     (db.Rates.Where(klmt => klmt.ClientCat_ID == rateId).Select(klm => klm.Rate_PerKM).FirstOrDefault()
                     * (decimal)quote.Quote_Distance);

                cost += db.Rates.Where(bsc => bsc.ClientCat_ID == rateId).Select(bsc => bsc.Base_Cost).FirstOrDefault();
                quote.Quote_Cost = cost;
                db.Entry(quote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = quote.Quote_ID });
            }
            ViewBag.ClientCat_ID = new SelectList(db.ClientCategories, "ClientCat_ID", "ClientCat_Type", quote.ClientCat_ID);
            return View(quote);
        }

        // GET: Quotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.Quotes.Find(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // POST: Quotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quote quote = db.Quotes.Find(id);
            db.Quotes.Remove(quote);
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
    }
}
