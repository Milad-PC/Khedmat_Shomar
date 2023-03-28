using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Khedmat_Shomar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //if have cookie send to counter
            return View();
        }
        public ActionResult Counter(DateTime FinishDate)
        {
            //set date time in cookie
            int Day = (FinishDate - DateTime.Now).Days;
            return View(Day);
        }
        [HttpPost]
        public ActionResult ShowFinish(string Roz,string Mah,string Sal,
            string RozKasri,string MahKasri,string RozBasij,string MahBasij,string Mojarad
            ,string Bache,string RozFarar,string RozEzaf)
        {


            return RedirectToAction("Counter", new { FinishDate = DateTime.Now });
        }
    }
}