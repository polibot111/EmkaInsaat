using EmreninSitesiMVC.Models.Project.DAL.Context;
using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using EmreninSitesiMVC.Models.VMClasses;
using Project.COMMON.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmreninSitesiMVC.Controllers
{
    public class HomeController : Controller
    {

        MyContext _db;

        public HomeController()
        {
            _db = new MyContext();
        }

        public ActionResult AnaSayfa()
        {
            return View();
        }

        public ActionResult AnaSayfaMail(string a)
        {
            ViewBag.MailGonderildi = TempData["uyari"];
            return View();
        }
        public ActionResult Index(string name, string number, string mail, string cont)
        {
            return View();
        }

        public ActionResult Deneme()
        {
            return View();
        }

        public ActionResult Sen()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IntDeneme(MailGonder mg)
        {
            int i = 1;
            if (TempData["sayi"] != null)
            {
                i = (int)TempData["sayi"];
            }           
            if (mg != null)
            {
                AppUser apa = _db.AppUsers.FirstOrDefault(x => x.ID == 1);

                string gonderilecekmail = $"{mg.FullName}\n{mg.Number}\n{mg.Maili}\n{mg.Number}";
                MailSender.Send(apa.Email, body: gonderilecekmail, subject: $"Emka Teklif {i}");
                i++;
                TempData["sayi"] = i;
                TempData["uyari"] = "Mailiniz Başarılı Bir Şekilde Gönderildi.";
            }
            else
            {
                TempData["uyari"] = "Mailiniz Gönderilemedi.";
            }
            return RedirectToAction("AnaSayfaMail" , new { a= "#contactus" });
        }

        public PartialViewResult IndexW(int id)
        {
            WorkProjectVM vm = new WorkProjectVM{

                WorkProjects = _db.WorkProjects.Where(x => x.CategoryID == id && x.Status != Models.Project.ENTITIES.Enums.Status.Deleted).ToList()
            };

             return PartialView("IndexW", vm);
            

        }

    }
}