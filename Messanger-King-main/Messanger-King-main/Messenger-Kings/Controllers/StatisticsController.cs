using Messenger_Kings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Messenger_Kings.Controllers
{
    public class StatisticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClientOrderStat()
        {
            //Shows total number of Orders in Db
            int totalOrd1 = (from p in db.Orders
                             select p).Count();

            ViewBag.totalOrders1 = totalOrd1.ToString();


            //Shows Total number of Clients 

            var roles1 = db.Roles.Where(r => r.Name == "Client");

            if (roles1.Any())
            {


                var roleIdd = roles1.First().Id;


                int totCli1 = (from r in db.Users
                               where r.Roles.Any(k => k.RoleId == roleIdd)
                               select r).Count();

                ViewBag.totCli1 = totCli1.ToString();

                //Gets array of all Client in the database
                var Cust1 = roles1.First().Id;

                var Customers1 = (from a in db.Users
                                  where a.Roles.Any(k => k.RoleId == Cust1)
                                  select a).ToList();

                ViewBag.Customers1 = Customers1.Count();


            }


            //Shows Total number of Types of Reports (On contract)
            int totContract1 = (from k in db.ClientCategories
                                where k.ClientCat_Type == "On Contract"
                                select k).Count();

            ViewBag.totContract1 = totContract1;

            //Shows Total number of Types of Reports (pay as you go)
            int totPay1 = (from k in db.ClientCategories
                           where k.ClientCat_Type == "Pay as you go"
                           select k).Count();

            ViewBag.totPay1 = totPay1;
            return View();
        }



        public ActionResult ChartAnalysis()
        {
            //Shows total number of Orders in Db
            int total = (from p in db.Orders
                         select p).Count();

            ViewBag.totalOrders = total.ToString();


            ////Shows Total number of Clients 

            //var roles = db.Roles.Where(r => r.Name == "Client");

            //if (roles.Any())
            //{


            //    var roleId = roles.First().Id;


            //    int totCli = (from r in db.Users
            //                  where r.Roles.Any(k => k.RoleId == roleId)
            //                  select r).Count();

            //    ViewBag.totCli = totCli.ToString();

            //    //Gets array of all Client in the database
            //    var Cust = roles.First().Id;

            //    var Customers = (from a in db.Users
            //                     where a.Roles.Any(k => k.RoleId == Cust)
            //                     select a).ToList();

            //    ViewBag.Customers = Customers.Count();


            //}


            //Shows Total number of Types of Reports (On contract)
            int totContract = (from k in db.ClientCategories
                               where k.ClientCat_Type == "On Contract"
                               select k).Count();

            ViewBag.totContract = totContract;

            //Shows Total number of Types of Reports (pay as you go)
            int totPay = (from k in db.ClientCategories
                          where k.ClientCat_Type == "Pay as you go"
                          select k).Count();

            ViewBag.totPay = totPay;


            //total orders each month(Jan)
            var jan = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 1 && p.Bookings.BookStatus == true).Count(); //total orders per month
            ViewBag.Jan = jan;

            var janBread = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 1 && p.Bookings.BookStatus == true); //getting all approved stat for jan

            //total orders each month(Feb)
            var feb = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 2 && p.Bookings.BookStatus == true).Count(); //total orders per month
            ViewBag.Feb = feb;

            var febBread = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 2 && p.Bookings.BookStatus == true); //getting all approved stat for feb

            //total orders each month(March)
            var march = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 3 && p.Bookings.BookStatus == true).Count(); //total orders per month
            ViewBag.March = march;

            var marchBread = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 3 && p.Bookings.BookStatus == true); //getting all approved stat for march

            //total orders each month(April)
            var apr = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 4 && p.Bookings.BookStatus == true).Count(); //total orders per month
            ViewBag.April = apr;

            var aprBread = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 4 && p.Bookings.BookStatus == true); //getting all approved stat for april

            //total orders each month(May)
            var may = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 5 && p.Bookings.BookStatus == true).Count(); //total orders per month
            ViewBag.May = may;

            var mayBread = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 5 && p.Bookings.BookStatus == true); //getting all approved stat for may


            //total orders each month(June)
            var june = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 6 && p.Bookings.BookStatus == true).Count(); //total orders per month
            ViewBag.June = june;

            var juneBread = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 6 && p.Bookings.BookStatus == true); //getting all approved stat for june

            //total orders each month(July)
            var july = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 7 && p.Bookings.BookStatus == true).Count(); //total orders per month
            ViewBag.July = july;

            var julyBread = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 7 && p.Bookings.BookStatus == true); //getting all approved stat for july

            //total orders each month(August)
            var aug = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 8 && p.Bookings.BookStatus == true).Count(); //total orders per month
            ViewBag.Aug = aug;

            var augBread = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 8 && p.Bookings.BookStatus == true); //getting all approved stat for august

            //total orders each month(September)
            var sept = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 9 && p.Bookings.BookStatus == true).Count(); //total orders per month
            ViewBag.Sept = sept;

            var septBread = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 9 && p.Bookings.BookStatus == true); //getting all approved stat for september

            //total orders each month(October)
            var oct = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 10 && p.Bookings.BookStatus == true).Count(); //total orders per month
            ViewBag.Oct = oct;

            var octBread = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 10 && p.Bookings.BookStatus == true); //getting all approved stat for october

            //total orders each month(November)
            var nov = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 11 && p.Bookings.BookStatus == true).Count(); //total orders per month
            ViewBag.Nov = nov;

            var novBread = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 11 && p.Bookings.BookStatus == true); //getting all approved stat for november

            //total orders each month(December)
            var dec = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 11 && p.Bookings.BookStatus == true).Count(); //total orders per month
            ViewBag.Dec = dec;

            var decBread = db.Orders.Include(p => p.Bookings).Where(p => p.Order_DateTime.Month == 11 && p.Bookings.BookStatus == true); //getting all approved stat for december

            //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //total amount per month (Jan)
            decimal totalJanBread = 0.0M;

            foreach (var Cri in janBread)
            {
                totalJanBread += Cri.Bookings.Book_TotalCost;
            }
            ViewBag.Jan1 = totalJanBread;

            //total amount per month (Feb)
            decimal totalFebBread = 0.0M;

            foreach (var Cri in febBread)
            {
                totalFebBread += Cri.Bookings.Book_TotalCost;
            }
            ViewBag.Feb1 = totalFebBread;

            //total amount per month (March)
            decimal totalMarchBread = 0.0M;

            foreach (var Cri in marchBread)
            {
                totalFebBread += Cri.Bookings.Book_TotalCost;
            }
            ViewBag.March1 = totalMarchBread;

            //total amount per month (April)
            decimal totalAprilBread = 0.0M;

            foreach (var Cri in aprBread)
            {
                totalAprilBread += Cri.Bookings.Book_TotalCost;
            }
            ViewBag.April1 = totalAprilBread;

            //total amount per month (May)
            decimal totalMayBread = 0.0M;

            foreach (var Cri in mayBread)
            {
                totalMayBread += Cri.Bookings.Book_TotalCost;
            }
            ViewBag.May1 = totalMayBread;

            //total amount per month (June)
            decimal totalJuneBread = 0.0M;

            foreach (var Cri in juneBread)
            {
                totalJuneBread += Cri.Bookings.Book_TotalCost;
            }
            ViewBag.June1 = totalAprilBread;

            //total amount per month (July)
            decimal totalJulyBread = 0.0M;

            foreach (var Cri in julyBread)
            {
                totalJulyBread += Cri.Bookings.Book_TotalCost;
            }
            ViewBag.July1 = totalJulyBread;

            //total amount per month (August)
            decimal totalAugBread = 0.0M;

            foreach (var Cri in augBread)
            {
                totalAugBread += Cri.Bookings.Book_TotalCost;
            }
            ViewBag.August1 = totalAugBread;

            //total amount per month (September)
            decimal totalSeptBread = 0.0M;

            foreach (var Cri in septBread)
            {
                totalSeptBread += Cri.Bookings.Book_TotalCost;
            }
            ViewBag.September1 = totalSeptBread;

            //total amount per month (October)
            decimal totalOctBread = 0.0M;

            foreach (var Cri in octBread)
            {
                totalOctBread += Cri.Bookings.Book_TotalCost;
            }
            ViewBag.October1 = totalOctBread;

            //total amount per month (November)
            decimal totalNovBread = 0.0M;

            foreach (var Cri in novBread)
            {
                totalNovBread += Cri.Bookings.Book_TotalCost;
            }
            ViewBag.November1 = totalNovBread;

            //total amount per month (December)
            decimal totalDecBread = 0.0M;

            foreach (var Cri in decBread)
            {
                totalDecBread += Cri.Bookings.Book_TotalCost;
            }
            ViewBag.December1 = totalDecBread;

            return View();
        }
    }
}

