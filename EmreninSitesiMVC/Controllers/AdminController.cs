using EmreninSitesiMVC.Models.DesignPatterns.SingletonPattern;
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
    public class AdminController : Controller
    {
        MyContext _db;

        public AdminController()
        {
            _db = DBTool.DBInstance;
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AppUser appUser, string panel)
        {

            AppUser ap = _db.AppUsers.FirstOrDefault(x => x.UserName == appUser.UserName);

            if (ap != null)
            {
                string decryped = DantexCrypt.DeCrypt(ap.Password);
                if (appUser.Password == decryped)
                {
                    Session["admin"] = ap;
                    if (panel == "1")
                    {
                        return RedirectToAction("Index", "Panel");
                    }
                    else
                    {
                        return RedirectToAction("AnaSayfa", "Company");
                    }

                }
                else
                {
                    ViewBag.Message = "Bilgiler Geçersiz";
                    return View();
                }
            }
            else
            {
                ViewBag.Message = "Bilgiler Geçersiz";
                return View();
            }

        }



        public ActionResult Forget()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Forget(AppUser appUser)
        {
            AppUser apa = _db.AppUsers.FirstOrDefault(x => x.Email == appUser.Email);
            if (apa != null)
            {
                appUser.ActivationCode = Guid.NewGuid();
                /*_db.Entry(apa).CurrentValues.SetValues(appUser);*///Aktivasyon kodu verilecek.
                apa.ActivationCode = appUser.ActivationCode;
                _db.SaveChanges();
                string gonderilecekMail = "Şifrenizi sıfırlamak için https://localhost:44333/Admin/PasswordReset/" + appUser.ActivationCode + " linkine tıklayın";
                MailSender.Send(appUser.Email, body: gonderilecekMail, subject: "Sifre Sıfırlama");
                ViewBag.Gonder = "Şifre sıfırlama mailiniz gönderildi.";
                return View();
            }
            ViewBag.HatalıMail = "Hatalı Mail";
            return View();
        }

        public ActionResult PasswordReset(Guid id)
        {
            AppUser ap = _db.AppUsers.FirstOrDefault(x => x.ActivationCode == id);

            if (ap != null)
            {
                TempData["id"] = ap.ID;
                return View();
            }
            ViewBag.Yanlis = "Hatalı Kod";
            return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public ActionResult PasswordReset(AppUser appUser)
        {
            if (appUser.Password != appUser.Control || appUser.Password == null && appUser.Email == null)
            {
                ViewBag.Eslesmedi = "Hatalı Girdiniz";
                return View();
            }
            int s = (int)TempData["id"];
            AppUser apa = _db.AppUsers.FirstOrDefault(x => x.ID == s);

            if (appUser.Email != null && appUser.Password == null)
            {
                appUser.EmailChange = Guid.NewGuid();
                apa.EmailChange = appUser.EmailChange;
                _db.SaveChanges();
                string emailChange = "Email değişimini onaylamak için https://localhost:44333/Admin/PasswordReset/" + appUser.EmailChange + " linkine tıklayın";
                MailSender.Send(apa.Email, body: emailChange, subject: "Email Değişimi");
                ViewBag.MailOnay = "Mail değişimi için mevcut emailinizden onaylayınız.";
            }
            else if (appUser.Email == null && appUser.Password != null)
            {
                apa.Password = DantexCrypt.Crypt(appUser.Password);
                _db.SaveChanges();
                string gonderilecekMail = "Şifreniz Değiştirildi." + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day;
                MailSender.Send(apa.Email, body: gonderilecekMail, subject: "Sifre Değiştirildi");

            }
            else
            {
                appUser.EmailChange = Guid.NewGuid();
                apa.EmailChange = appUser.EmailChange;
                apa.Password = DantexCrypt.Crypt(appUser.Password);
                _db.SaveChanges();
                TempData["email"] = appUser.EmailChange;
                string emailChange = "Şifreniz başarılı bir şekilde değiştirildi. Email değişimini onaylamak için https://localhost:44333/Admin/MailChange/" + appUser.EmailChange + " linkine tıklayın";
                MailSender.Send(apa.Email, body: emailChange, subject: "Şifreniz Değişti. Mail Değişimi");
                ViewBag.MailOnay = "Şifreni başarılı bir şekilde değiştirildi. Mail değişimi için mevcut emailinizden onaylayınız.";


            }




            return RedirectToAction("Login", "Admin");

        }

        public ActionResult MailChange(Guid id)
        {
            AppUser ap = _db.AppUsers.FirstOrDefault(x => x.EmailChange == id);
            ap.Email = TempData["email"].ToString();
            _db.SaveChanges();
            return RedirectToAction("Login", "Admin");
        }



    }
}