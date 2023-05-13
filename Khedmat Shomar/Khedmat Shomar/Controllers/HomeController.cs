using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using Khedmat_Shomar.DateBase;

namespace Khedmat_Shomar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DateTime finishDate = DateTime.Now.AddYears(2);
            //if have cookie send to counter
            HttpCookie cookie = Request.Cookies["KhedmatShomar"];
            if (cookie != null)
            {
                if (cookie.Value != string.Empty)
                {
                    try
                    {
                        finishDate = Convert.ToDateTime(cookie.Value);
                    }
                    catch { }
                    return RedirectToAction("Counter", new { FinishDate = finishDate });
                }
            }

            return View();
        }
        public ActionResult Counter(DateTime FinishDate)
        {
            HttpCookie cookie = Request.Cookies["KhedmatShomar"];
            if (cookie == null || cookie.Value == string.Empty)
            {
                HttpCookie MyCookies = new HttpCookie("KhedmatShomar");
                MyCookies.Value = FinishDate.ToString();
                MyCookies.Expires = DateTime.Now.AddDays(90);
                Response.SetCookie(MyCookies);
            }

            int Day = (FinishDate - DateTime.Now).Days;

            // Insert Visit
            Visit src = new Visit();
            src.IP = (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
            src.Browser = Request.Browser.Browser;
            src.OS = Request.UserAgent;
            src.VisitDate = DateTime.Now;
            MyContext db = new MyContext();
            db.Visits.Add(src);
            db.SaveChanges();

            ViewBag.Visit = db.Visits.Count().ToString("n0");
            // End Insert Visit


            return View(Day);
        }

        [HttpPost]
        public ActionResult ShowFinish(Soldier input)
        {
            PersianCalendar cal = new PersianCalendar();
            DateTime dt = new DateTime(Convert.ToInt32(input.Sal),
                Convert.ToInt32(input.Mah),
                Convert.ToInt32(input.Roz), cal);
            dt = dt.AddYears(2);
            //RozKasri
            if (input.RozKasri != String.Empty)
            {
                try
                {
                    dt = dt.AddDays(Convert.ToInt32(input.RozKasri) * (-1));
                }
                catch { }
            }
            // MahKasri
            if (input.MahKasri != String.Empty)
            {
                try
                {
                    dt = dt.AddMonths(Convert.ToInt32(input.MahKasri) * (-1));
                }
                catch { }
            }
            // RozBasij
            if (input.RozBasij != String.Empty)
            {
                try
                {
                    dt = dt.AddDays(Convert.ToInt32(input.RozBasij) * (-1));
                }
                catch { }
            }
            // MahBasij
            if (input.MahBasij != String.Empty)
            {
                try
                {
                    dt = dt.AddMonths(Convert.ToInt32(input.MahBasij) * (-1));
                }
                catch { }
            }

            // motaehel
            if (input.Mojarad == "2")
            {
                dt = dt.AddMonths(-2);
            }
            // Bache
            if (input.Bache != String.Empty)
            {
                try
                {
                    dt = dt.AddMonths(Convert.ToInt32(input.Bache) * (-3));
                }
                catch { }
            }


            // RozFarar
            if (input.RozFarar != String.Empty)
            {
                try
                {
                    dt = dt.AddMonths(Convert.ToInt32(input.RozFarar) * 3);
                }
                catch { }
            }

            // RozEzaf
            if (input.RozEzaf != String.Empty)
            {
                try
                {
                    dt = dt.AddMonths(Convert.ToInt32(input.RozEzaf) * 3);
                }
                catch { }
            }
            
            return RedirectToAction("Counter", new { FinishDate = dt });
        }

        public ActionResult ShowVisits()
        {
            MyContext db = new MyContext();

            IEnumerable<Visit> visits = db.Visits.OrderByDescending(u=>u.VisitDate);
            return View(visits);
        }
    }
}