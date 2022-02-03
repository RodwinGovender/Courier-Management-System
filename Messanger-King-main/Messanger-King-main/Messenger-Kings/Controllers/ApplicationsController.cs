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

namespace Messenger_Kings.Controllers
{
    public class ApplicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Applications
        public ActionResult Index()
        {
            return View(db.Applications.ToList());
        }

        // GET: Applications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // GET: Applications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "App_ID,App_AdminComment,IDCard")] Application application, HttpPostedFileBase filelist, HttpPostedFileBase pdfList)
        {
            if (ModelState.IsValid)
            {
                    String FileExt = Path.GetExtension(filelist.FileName).ToUpper();
                    String PDFExt = Path.GetExtension(pdfList.FileName).ToUpper();
                    if (PDFExt == ".PDF")
                    {
                        Stream str = pdfList.InputStream;
                        BinaryReader Br = new BinaryReader(str);
                        Byte[] FileDet = Br.ReadBytes((Int32)str.Length);



                        application.BankStatement = FileDet;


                    }

                    if(FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                    {


                            application.IDCard = ConvertToBytes(filelist);


                       

                    }
                    db.Applications.Add(application);
                    db.SaveChanges();
              
                    ViewBag.FileStatus = "Invalid file format.";
 


                return RedirectToAction("Index");
            }

            return View(application);
        }

        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            BinaryReader reader = new BinaryReader(file.InputStream);
            return reader.ReadBytes((int)file.ContentLength);
        }

        [HttpGet]
        public FileResult DownLoadFile(int id)
        {


            ApplicationDbContext db = new ApplicationDbContext();

            var FileById = (from FC in db.Applications
                            where FC.App_ID.Equals(id)
                            select new { FC.BankStatement }).ToList().FirstOrDefault();

            return File(FileById.BankStatement, "application/pdf");

        }

        // GET: Applications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "App_ID,App_AdminComment,IDCard")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(application);
        }

        // GET: Applications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
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
