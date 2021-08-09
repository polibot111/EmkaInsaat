using EmreninSitesiMVC.AuthenticationClasses;
using EmreninSitesiMVC.Models.DesignPatterns.SingletonPattern;
using EmreninSitesiMVC.Models.Project.DAL.Context;
using EmreninSitesiMVC.Models.Project.ENTITIES.Enums;
using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using EmreninSitesiMVC.Models.VMClasses.WorkPlace;
using iTextSharp.tool.xml.html;
using javax.swing.text.html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTML = javax.swing.text.html.HTML;



namespace EmreninSitesiMVC.Controllers
{
    [AdminAuthentication]
    public class CompanyController : Controller
    {
        MyContext _db;
        EAVM ea;
        SirketVM ss;
        CekVM cvm;
        DateTime kk;

        public CompanyController()
        {
            _db = DBTool.DBInstance;
        }
        public ActionResult Index()
        {
            return View();
        }
        // İş Tablosu -------------------------------
        public ActionResult IsTablosu(int? ay, int? yil)
        {
            try
            {
                if (ay == 0 && yil != 0)
                {
                    ss = new SirketVM
                    {
                        YapilanIss = _db.YapilanIss.Where(x => x.CreatedTime.Year == yil).ToList(),
                    };

                    ViewBag.Message = $"{TempData["ResultMessage"]}";

                }
                else if (yil == 0 && ay != 0)
                {
                    ss = new SirketVM
                    {
                        YapilanIss = _db.YapilanIss.Where(x => x.CreatedTime.Month == ay).ToList(),
                    };

                    ViewBag.Message = $"{TempData["ResultMessage"]}";
                }
                else if (yil != 0 && ay != 0 && ay != null)
                {
                    ss = new SirketVM
                    {
                        YapilanIss = _db.YapilanIss.Where(x => x.CreatedTime.Month == ay && x.CreatedTime.Year == yil).ToList(),
                    };

                    ViewBag.Message = $"{TempData["ResultMessage"]}";
                }
                else
                {
                    ss = new SirketVM
                    {
                        YapilanIss = _db.YapilanIss.ToList(),
                    };

                    ViewBag.Message = $"{TempData["ResultMessage"]}";
                }

            }
            catch (Exception)
            {

                throw;
            }



            return View(ss);
        }
        [HttpPost]
        public ActionResult IsTablosu(SirketVM ss)
        {
            try
            {
                decimal c = 0;
                decimal g = 0;
                if (ss.Cek != null)
                {
                    foreach (Maliyet item in ss.YapilanIs.Maliyets)
                    {
                        g += item.ToplamMaliyet;

                    }
                    foreach (Cek item in ss.Cek)
                    {
                        if (item.CekNo != null && item.Tutar != 0)
                        {
                            c += item.Tutar;
                            ss.YapilanIs.Cek.Add(item);
                        }
                    }

                    ss.YapilanIs.KalanOdeme = ss.YapilanIs.FaturaFiyat - (ss.YapilanIs.YapilanIsOdemes[0].Odeme + c);

                    if (ss.YapilanIs.KalanOdeme == 0 || ss.YapilanIs.KalanOdeme < 0)
                    {
                        ss.YapilanIs.OdemeDurumu = OdemeDurumu.Odendi;
                    }
                    else if (ss.YapilanIs.YapilanIsOdemes[0].Odeme + c == 0)
                    {
                        ss.YapilanIs.OdemeDurumu = OdemeDurumu.Odenmedi;
                    }
                    else
                    {
                        ss.YapilanIs.OdemeDurumu = OdemeDurumu.YarımOdendi;
                    }
                    ss.YapilanIs.Kar = ss.YapilanIs.FaturaFiyat - g;
                    _db.YapilanIss.Add(ss.YapilanIs);
                    _db.SaveChanges();

                }
                else
                {
                    foreach (Maliyet item in ss.YapilanIs.Maliyets)
                    {
                        item.KalanOdeme = item.ToplamMaliyet - item.MaliyetOdemes[0].Odeme;
                        g += item.ToplamMaliyet;

                    }
                    ss.YapilanIs.TotalOdeme = ss.YapilanIs.YapilanIsOdemes[0].Odeme + c;
                    ss.YapilanIs.KalanOdeme = ss.YapilanIs.FaturaFiyat - ss.YapilanIs.TotalOdeme;

                    if (ss.YapilanIs.KalanOdeme == 0)
                    {
                        ss.YapilanIs.OdemeDurumu = OdemeDurumu.Odendi;
                    }
                    else if (ss.YapilanIs.YapilanIsOdemes[0].Odeme + c == 0)
                    {
                        ss.YapilanIs.OdemeDurumu = OdemeDurumu.Odenmedi;
                    }
                    else
                    {
                        ss.YapilanIs.OdemeDurumu = OdemeDurumu.YarımOdendi;
                    }
                    ss.YapilanIs.Kar = ss.YapilanIs.FaturaFiyat - g;
                    ss.YapilanIs.PageType = PageType.WorkPlace;
                    _db.YapilanIss.Add(ss.YapilanIs);
                    _db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("IsTablosu");
        }
        [HttpPost]
        public ActionResult MaliyetEkle(int id, string deger, string borc, string ilkodeme)
        {
            try
            {
                if (deger != "" && borc != "")
                {
                    string[] ss = borc.Split('.');
                    if (ss.Length > 1)
                    {
                        borc = $"{ss[0]},{ss[1]}";
                    }
                    string[] sz = ilkodeme.Split('.');
                    if (sz.Length > 1)
                    {
                        ilkodeme = $"{sz[0]},{sz[1]}";
                    }
                    decimal z = 0;
                    YapilanIs yi = _db.YapilanIss.Find(id);
                    Maliyet zz = new Maliyet();
                    zz.Aciklama = deger;
                    zz.ToplamMaliyet = Convert.ToDecimal(borc);
                    zz.KalanOdeme = zz.ToplamMaliyet;
                    zz.OdemeDurumu = OdemeDurumu.Odenmedi;
                    if (ilkodeme != "")
                    {
                        MaliyetOdeme mo = new MaliyetOdeme();
                        mo.Odeme = Convert.ToDecimal(ilkodeme);
                        zz.KalanOdeme = zz.ToplamMaliyet - mo.Odeme;
                        zz.MaliyetOdemes.Add(mo);
                        zz.OdemeDurumu = OdemeDurumu.YarımOdendi;
                    }
                    zz.PageType = PageType.WorkPlace;
                    yi.Maliyets.Add(zz);
                    foreach (Maliyet item in yi.Maliyets)
                    {
                        z += item.ToplamMaliyet;
                    }
                    yi.Kar = yi.FaturaFiyat - z;
                    _db.SaveChanges();
                }
                else
                {
                    TempData["ResultMessage"] = "Borç Açıklaması ve Borç Boş Olamaz";
                }
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("IsTablosu");
        }
        public ActionResult UpdateYapilanIs(int? id)
        {
            SirketVM vm = new SirketVM
            {
                YapilanIs = _db.YapilanIss.Find(id),
            };
            return View(vm);
        }
        [HttpPost]
        public ActionResult UpdateYapilanIs(SirketVM ss)
        {
            try
            {
                kk = DateTime.Now;
                string gosterge = kk.ToString("dd/MM/yyyy HH:mm");
                decimal y = 0;
                decimal g = 0;
                YapilanIs zz = _db.YapilanIss.Find(ss.YapilanIs.ID);
                zz.SirketAdi = ss.YapilanIs.SirketAdi;
                zz.Aciklama = ss.YapilanIs.Aciklama;
                zz.Temsilcisi = ss.YapilanIs.Temsilcisi;
                zz.OdemeSekli = ss.YapilanIs.OdemeSekli;
                zz.FaturaNo = ss.YapilanIs.FaturaNo;
                zz.FaturaFiyat = ss.YapilanIs.FaturaFiyat;
                zz.FaturaAcıklama = ss.YapilanIs.FaturaAcıklama;
                zz.HakedisNo = ss.YapilanIs.HakedisNo;
                zz.HakedisFiyat = ss.YapilanIs.HakedisFiyat;
                zz.IsDurumu = ss.YapilanIs.IsDurumu;
                zz.OdemeDurumu = ss.YapilanIs.OdemeDurumu;
                for (int i = 0; i < zz.Cek.Count; i++)
                {
                    zz.Cek[i].CekNo = ss.YapilanIs.Cek[i].CekNo;
                    zz.Cek[i].Tutar = ss.YapilanIs.Cek[i].Tutar;
                    if (gosterge != ss.YapilanIs.Cek[i].AlisTarihi.ToString("dd/MM/yyyy HH:mm"))
                    {
                        zz.Cek[i].AlisTarihi = ss.YapilanIs.Cek[i].AlisTarihi;
                        zz.Cek[i].TahsilatTarihi = ss.YapilanIs.Cek[i].TahsilatTarihi;
                    }

                    zz.Cek[i].CekSenet = ss.YapilanIs.Cek[i].CekSenet;

                    if (zz.Cek[i].TahsilDurumu == TahsilDurumu.Alındı)
                    {
                        y += zz.Cek[i].Tutar;
                    }
                }
                for (int i = 0; i < zz.Maliyets.Count; i++)
                {
                    zz.Maliyets[i].Aciklama = ss.YapilanIs.Maliyets[i].Aciklama;
                    zz.Maliyets[i].ToplamMaliyet = ss.YapilanIs.Maliyets[i].ToplamMaliyet;
                    g += zz.Maliyets[i].ToplamMaliyet;
                }
                for (int i = 0; i < zz.YapilanIsOdemes.Count; i++)
                {
                    if (ss.YapilanIs.YapilanIsOdemes[i].Odeme != 0)
                    {
                        zz.YapilanIsOdemes[i].Odeme = ss.YapilanIs.YapilanIsOdemes[i].Odeme;
                    }
                    y += zz.YapilanIsOdemes[i].Odeme;
                }
                zz.TotalOdeme = y;
                zz.Kar = zz.FaturaFiyat - g;
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("IsTablosu");
        }

        public PartialViewResult AyrintiPenceresi(int id)
        {

            YapilanIs yi = _db.YapilanIss.Find(id);
            return PartialView(yi);
        }
        [HttpPost]
        public ActionResult OdemeEkleme(int id, string deger)
        {
            if (deger != "")
            {
                string[] ss = deger.Split('.');
                if (ss.Length > 1)
                {
                    deger = $"{ss[0]},{ss[1]}";
                }
                YapilanIs yi = _db.YapilanIss.Find(id);
                decimal z = Convert.ToDecimal(deger);
                decimal y = 0;
                YapilanIsOdeme yio = new YapilanIsOdeme();
                yio.Odeme = z;
                yi.YapilanIsOdemes.Add(yio);
                foreach (YapilanIsOdeme item in yi.YapilanIsOdemes)
                {
                    y += item.Odeme;
                }
                foreach (Cek item in yi.Cek)
                {
                    if (item.TahsilDurumu == TahsilDurumu.Alındı)
                    {
                        y += item.Tutar;
                    }
                }
                yi.TotalOdeme = y;
                yi.OdemeDurumu = OdemeDurumu.YarımOdendi;
                if (yi.TotalOdeme == yi.FaturaFiyat)
                {
                    yi.OdemeDurumu = OdemeDurumu.Odendi;
                }
                _db.SaveChanges();
            }


            return RedirectToAction("IsTablosu");
        }

        public ActionResult DeleteIs(int id)
        {
            try
            {
                YapilanIs ys = _db.YapilanIss.Find(id);
                if (ys.Cek.Count != 0)
                {
                    int cc = ys.Cek.Count;
                    for (int i = 0; i < cc; i++)
                    {

                        _db.Ceks.Remove(ys.Cek[0]);
                    }
                }
                if (ys.Maliyets.Count != 0)
                {
                    int mc = ys.Maliyets.Count;
                    for (int i = 0; i < mc; i++)
                    {
                        _db.Maliyets.Remove(ys.Maliyets[0]);
                    }
                }
                if (ys.YapilanIsOdemes.Count != 0)
                {
                    int yio = ys.YapilanIsOdemes.Count;
                    for (int i = 0; i < yio; i++)
                    {
                        _db.YapilanIsOdemes.Remove(ys.YapilanIsOdemes[0]);
                    }
                }
                _db.YapilanIss.Remove(ys);
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }


            return RedirectToAction("IsTablosu");
        }

        // İş Tablosu -----------------------------

        // Çek Controller Section --------------------------------------------------------------
        public ActionResult Cek(int? ay, int? yil, int? tur)
        {
            try
            {
                if (tur == 1)
                {
                    if (ay == 0 && yil != 0)
                    {
                        cvm = new CekVM
                        {
                            Ceks = _db.Ceks.Where(x => x.AlisTarihi.Year == yil).ToList(),
                            YapilanIss = _db.YapilanIss.ToList()

                        };

                    }
                    else if (yil == 0 && ay != 0)
                    {
                        cvm = new CekVM
                        {
                            Ceks = _db.Ceks.Where(x => x.AlisTarihi.Month == ay).ToList(),
                            YapilanIss = _db.YapilanIss.ToList()

                        };
                    }
                    else if (yil != 0 && ay != 0 && ay != null)
                    {
                        cvm = new CekVM
                        {
                            Ceks = _db.Ceks.Where(x => x.AlisTarihi.Month == ay && x.AlisTarihi.Year == yil).ToList(),
                            YapilanIss = _db.YapilanIss.ToList()

                        };
                    }
                    else
                    {
                        cvm = new CekVM
                        {
                            Ceks = _db.Ceks.ToList(),
                            YapilanIss = _db.YapilanIss.ToList()

                        };
                    }
                }
                else if (tur == 2)
                {
                    if (ay == 0 && yil != 0)
                    {
                        cvm = new CekVM
                        {
                            Ceks = _db.Ceks.Where(x => x.TahsilatTarihi.Year == yil).ToList(),
                            YapilanIss = _db.YapilanIss.ToList()

                        };

                    }
                    else if (yil == 0 && ay != 0)
                    {
                        cvm = new CekVM
                        {
                            Ceks = _db.Ceks.Where(x => x.TahsilatTarihi.Month == ay).ToList(),
                            YapilanIss = _db.YapilanIss.ToList()

                        };
                    }
                    else if (yil != 0 && ay != 0 && ay != null)
                    {
                        cvm = new CekVM
                        {
                            Ceks = _db.Ceks.Where(x => x.TahsilatTarihi.Month == ay && x.TahsilatTarihi.Year == yil).ToList(),
                            YapilanIss = _db.YapilanIss.ToList()

                        };
                    }
                    else
                    {
                        cvm = new CekVM
                        {
                            Ceks = _db.Ceks.ToList(),
                            YapilanIss = _db.YapilanIss.ToList()

                        };
                    }
                }
                else
                {
                    cvm = new CekVM
                    {
                        Ceks = _db.Ceks.ToList(),
                        YapilanIss = _db.YapilanIss.ToList()

                    };
                }

            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }

            return View(cvm);
        }
        [HttpPost]
        public ActionResult Cek(CekVM cc, int id)
        {
            try
            {
                if (id != 0)
                {
                    cc.Cek.YapilanIsID = id;
                    cc.Cek.YapilanIs = _db.YapilanIss.Find(id);
                    _db.Ceks.Add(cc.Cek);
                    cc.Cek.YapilanIs.KalanOdeme -= cc.Cek.Tutar;
                    _db.SaveChanges();
                }
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }


            return RedirectToAction("Cek");
        }

        public ActionResult TahsilCek(int id, string a)
        {
            try
            {
                if (a == null)
                {
                    Cek cs = _db.Ceks.Find(id);
                    cs.TahsilDurumu = TahsilDurumu.Alındı;
                    _db.SaveChanges();
                    return RedirectToAction("Cek");
                }
                else
                {
                    Cek cs = _db.Ceks.Find(id);
                    cs.TahsilDurumu = TahsilDurumu.Alındı;
                    _db.SaveChanges();
                    return RedirectToAction("AnaSayfa");
                }
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
                if (a == null)
                {

                    return RedirectToAction("Cek");
                }
                else
                {

                    return RedirectToAction("AnaSayfa");
                }
            }


        }
        public ActionResult UpdateCek(int id)
        {
            try
            {
                cvm = new CekVM()
                {
                    Cek = _db.Ceks.Find(id),
                    YapilanIss = _db.YapilanIss.ToList()

                };
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }

            return View(cvm);
        }
        [HttpPost]
        public ActionResult UpdateCek(CekVM cc, int id)
        {
            try
            {
                Cek ss = _db.Ceks.Find(cc.Cek.ID);
                ss.CekNo = cc.Cek.CekNo;
                ss.TahsilDurumu = cc.Cek.TahsilDurumu;
                if (cc.Cek.Tutar != 0)
                {
                    ss.Tutar = cc.Cek.Tutar;
                }
                kk = DateTime.Now;
                if (kk.ToString("MM/dd/yyyy HH:mm") != cc.Cek.TahsilatTarihi.ToString("MM/dd/yyyy HH:mm"))
                {
                    ss.TahsilatTarihi = cc.Cek.TahsilatTarihi;
                }
                if (id != 0)
                {
                    ss.YapilanIs.KalanOdeme += ss.Tutar;
                    ss.YapilanIsID = cc.Cek.YapilanIsID;
                    ss.YapilanIs = _db.YapilanIss.Find(id);
                    ss.YapilanIs.KalanOdeme -= ss.Tutar;
                };
                _db.SaveChanges();
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }


            return RedirectToAction("Cek");
        }


        public ActionResult DeleteCek(int id)
        {
            try
            {
                Cek cc = _db.Ceks.Find(id);
                if (cc.TahsilDurumu == TahsilDurumu.Alındı)
                {
                    cc.YapilanIs.TotalOdeme -= cc.Tutar;
                    cc.YapilanIs.KalanOdeme += cc.Tutar;
                }
                _db.Ceks.Remove(cc);
                _db.SaveChanges();
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }

            return RedirectToAction("Cek");
        }
        //-----------------------------------------------------------------------------------------

        // EA Section ----------------------------------------------------
        public ActionResult EA(int? ay, int? yil, int? tur)
        {
            try
            {
                if (tur != null)
                {
                    if (tur == 1)
                    {
                        if (ay == 0 && yil != 0)
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.CreatedTime.Month == ay && x.BorcTuru == BorcTuru.Araba).ToList(),
                            };

                        }
                        else if (yil == 0 && ay != 0)
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.CreatedTime.Year == yil && x.BorcTuru == BorcTuru.Araba).ToList(),
                            };
                        }
                        else if (yil != 0 && ay != 0 && ay != null)
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.CreatedTime.Month == ay && x.CreatedTime.Year == yil && x.BorcTuru == BorcTuru.Araba).ToList(),
                            };
                        }
                        else
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.BorcTuru == BorcTuru.Araba).ToList(),
                            };
                        }
                    }
                    else if (tur == 2)
                    {
                        if (ay == 0 && yil != 0)
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.CreatedTime.Month == ay && x.BorcTuru == BorcTuru.SirketCalısanı).ToList(),
                            };

                        }
                        else if (yil == 0 && ay != 0)
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.CreatedTime.Year == yil && x.BorcTuru == BorcTuru.SirketCalısanı).ToList(),
                            };
                        }
                        else if (yil != 0 && ay != 0 && ay != null)
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.CreatedTime.Month == ay && x.CreatedTime.Year == yil && x.BorcTuru == BorcTuru.SirketCalısanı).ToList(),
                            };
                        }
                        else
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.BorcTuru == BorcTuru.SirketCalısanı).ToList(),
                            };
                        }
                    }
                    else if (tur == 3)
                    {
                        if (ay == 0 && yil != 0)
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.CreatedTime.Month == ay && x.BorcTuru == BorcTuru.Diger).ToList(),
                            };

                        }
                        else if (yil == 0 && ay != 0)
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.CreatedTime.Year == yil && x.BorcTuru == BorcTuru.Diger).ToList(),
                            };
                        }
                        else if (yil != 0 && ay != 0 && ay != null)
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.CreatedTime.Month == ay && x.CreatedTime.Year == yil && x.BorcTuru == BorcTuru.Diger).ToList(),
                            };
                        }
                        else
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.BorcTuru == BorcTuru.Diger).ToList(),
                            };
                        }
                    }
                    else
                    {
                        if (ay == 0 && yil != 0)
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.CreatedTime.Month == ay).ToList(),
                            };

                        }
                        else if (yil == 0 && ay != 0)
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.CreatedTime.Year == yil).ToList(),
                            };
                        }
                        else if (yil != 0 && ay != 0 && ay != null)
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.Where(x => x.CreatedTime.Month == ay && x.CreatedTime.Year == yil).ToList(),
                            };
                        }
                        else
                        {
                            ea = new EAVM
                            {
                                ListEA = _db.EABorcs.ToList(),
                            };
                        }
                    }
                }
                else
                {
                    ea = new EAVM
                    {
                        ListEA = _db.EABorcs.ToList(),
                    };
                }

            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }


            return View(ea);
        }
        [HttpPost]
        public ActionResult EA(EAVM ea)
        {
            try
            {
                if (ea.SingleEA.Borc != 0 && ea.SingleEA.Kime != null)
                {
                    if (ea.SingleOdeme.Odeme != 0 && ea.SingleOdeme.Odeme != ea.SingleEA.Borc)
                    {
                        ea.SingleEA.OdemeDurumu = Models.Project.ENTITIES.Enums.OdemeDurumu.YarımOdendi;
                        ea.SingleEA.EABorcOdemes.Add(ea.SingleOdeme);
                        _db.EABorcs.Add(ea.SingleEA);
                        _db.SaveChanges();
                    }
                    else if (ea.SingleOdeme.Odeme == 0)
                    {
                        ea.SingleEA.OdemeDurumu = Models.Project.ENTITIES.Enums.OdemeDurumu.Odenmedi;
                        _db.EABorcs.Add(ea.SingleEA);
                        _db.SaveChanges();
                    }
                    else
                    {
                        ea.SingleEA.OdemeDurumu = Models.Project.ENTITIES.Enums.OdemeDurumu.Odendi;
                        ea.SingleEA.EABorcOdemes.Add(ea.SingleOdeme);
                        _db.EABorcs.Add(ea.SingleEA);
                        _db.SaveChanges();
                    }
                }


            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }

            return RedirectToAction("EA");

        }
        public ActionResult DeleteEA(int id)
        {
            try
            {
                _db.EABorcs.Remove(_db.EABorcs.Find(id));
                _db.SaveChanges();
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }

            return RedirectToAction("EA");
        }
        public ActionResult UpdateEA(int? id)
        {
            try
            {
                ea = new EAVM
                {
                    SingleEA = _db.EABorcs.Find(id),

                };
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }

            return PartialView(ea);
        }
        [HttpPost]
        public ActionResult UpdateEA(EAVM ea)
        {
            try
            {
                EABorc b1 = _db.EABorcs.Find(ea.SingleEA.ID);

                b1.Kime = ea.SingleEA.Kime;
                b1.Aciklama = ea.SingleEA.Aciklama;
                b1.Borc = ea.SingleEA.Borc;
                b1.BorcTuru = ea.SingleEA.BorcTuru;
                for (int i = 0; i < ea.SingleEA.EABorcOdemes.Count; i++)
                {
                    if (ea.SingleEA.EABorcOdemes[i].Odeme != 0)
                    {
                        b1.EABorcOdemes[i].Odeme = ea.SingleEA.EABorcOdemes[i].Odeme;
                    }

                }
                if (b1.Borc == b1.EABorcOdemes.Sum(x => x.Odeme))
                {
                    b1.OdemeDurumu = OdemeDurumu.Odendi;
                }
                else if (b1.EABorcOdemes.Sum(x => x.Odeme) == 0)
                {
                    b1.OdemeDurumu = OdemeDurumu.Odenmedi;
                }
                else
                {
                    b1.OdemeDurumu = OdemeDurumu.YarımOdendi;
                }
                _db.SaveChanges();
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }

            return RedirectToAction("EA");
        }
        public PartialViewResult PartialGraph()
        {

            return PartialView();
        }
        [HttpPost]
        public ActionResult PartialTakip(int id, string deger)
        {
            try
            {
                if (deger != null || deger != "0")
                {
                    string[] ss = deger.Split('.');
                    if (ss.Length > 1)
                    {
                        deger = $"{ss[0]},{ss[1]}";
                    }
                    decimal odeme = Convert.ToDecimal(deger);
                    decimal z = 0;
                    EABorc borc = _db.EABorcs.Find(id);
                    EABorcOdeme bo = new EABorcOdeme();
                    bo.Odeme = odeme;
                    borc.EABorcOdemes.Add(bo);
                    foreach (EABorcOdeme item in borc.EABorcOdemes)
                    {
                        z += item.Odeme;
                    }
                    if (0 == borc.Borc - z) borc.OdemeDurumu = Models.Project.ENTITIES.Enums.OdemeDurumu.Odendi;
                    else if (0 != borc.Borc - z) borc.OdemeDurumu = Models.Project.ENTITIES.Enums.OdemeDurumu.YarımOdendi;
                    _db.SaveChanges();
                }
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }


            return RedirectToAction("EA");
        }
        [HttpPost]
        public ActionResult FiyatGuncelle(int id, string deger)
        {
            try
            {
                if (deger != null || deger != "0")
                {
                    decimal odeme = Convert.ToDecimal(deger);
                    EABorcOdeme bo = _db.EABorcOdemes.Find(id);
                    bo.Odeme = odeme;
                    _db.SaveChanges();
                }
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }

            return RedirectToAction("EA");
        }

        public ActionResult SamimiyetTablosu(int? ay, int? yil)
        {
            try
            {
                if (ay == 0 && yil != 0)
                {
                    SirketVM vm = new SirketVM()
                    {
                        YapilanIss = _db.YapilanIss.Where(x => x.CreatedTime.Year == yil).ToList(),
                    };
                    return View(vm);

                }
                else if (yil == 0 && ay != 0)
                {
                    SirketVM vm = new SirketVM()
                    {
                        YapilanIss = _db.YapilanIss.Where(x => x.CreatedTime.Month == ay).ToList(),
                    };
                    return View(vm);
                }
                else if (yil != 0 && ay != 0 && ay != null)
                {
                    SirketVM vm = new SirketVM()
                    {
                        YapilanIss = _db.YapilanIss.Where(x => x.CreatedTime.Month == ay && x.CreatedTime.Year == yil).ToList(),
                    };
                    return View(vm);
                }
                else
                {
                    SirketVM vm = new SirketVM()
                    {
                        YapilanIss = _db.YapilanIss.ToList(),
                    };
                    return View(vm);
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
                return View(); ;
            }


        }
        // Maliyet Section ----------------

        public ActionResult MaliyetTablosu(int? ay, int? yil)
        {
            try
            {
                if (ay == 0 && yil != 0)
                {
                    MaliyetVM mv = new MaliyetVM()
                    {
                        YapilanIss = _db.YapilanIss.ToList(),
                        Maliyets = _db.Maliyets.Where(x => x.CreatedTime.Year == yil).ToList()
                    };
                    return View(mv);

                }
                else if (yil == 0 && ay != 0)
                {
                    {
                        MaliyetVM mv = new MaliyetVM()
                        {
                            YapilanIss = _db.YapilanIss.ToList(),
                            Maliyets = _db.Maliyets.Where(x => x.CreatedTime.Month == ay).ToList()
                        };
                        return View(mv);
                    }
                }
                else if (yil != 0 && ay != 0 && ay != null)
                {
                    MaliyetVM mv = new MaliyetVM()
                    {
                        YapilanIss = _db.YapilanIss.ToList(),
                        Maliyets = _db.Maliyets.Where(x => x.CreatedTime.Month == ay && x.CreatedTime.Year == yil).ToList()
                    };
                    return View(mv);
                }
                else
                {
                    MaliyetVM mv = new MaliyetVM()
                    {
                        YapilanIss = _db.YapilanIss.ToList(),
                        Maliyets = _db.Maliyets.ToList()
                    };
                    return View(mv);
                }
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
                return View();
            }


        }
        [HttpPost]
        public ActionResult MaliyetOde(int id, string deger)
        {
            try
            {
                if (deger != "")
                {
                    decimal y = 0;
                    decimal h = Convert.ToDecimal(deger);
                    Maliyet m = _db.Maliyets.Find(id);
                    MaliyetOdeme mo = new MaliyetOdeme();
                    mo.Odeme = h;
                    m.MaliyetOdemes.Add(mo);
                    m.KalanOdeme = m.ToplamMaliyet - m.KalanOdeme; 
                    m.KalanOdeme -= h;
                    if (m.KalanOdeme <= 0)
                    {
                        m.OdemeDurumu = OdemeDurumu.Odendi;
                    }
                    else
                    {
                        m.OdemeDurumu = OdemeDurumu.YarımOdendi;
                    }
                    _db.SaveChanges();
                }
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }


            return RedirectToAction("MaliyetTablosu");
        }
        public ActionResult DeleteMal(int id)
        {
            try
            {
                Maliyet mm = _db.Maliyets.Find(id);
                mm.YapilanIs.Kar += mm.ToplamMaliyet;
                _db.Maliyets.Remove(mm);
                _db.SaveChanges();
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }

            return RedirectToAction("MaliyetTablosu");
        }
        public ActionResult UpdateMal(int id)
        {
            try
            {
                var iss = _db.YapilanIss.Select(x => new { id = x.ID, Name = x.SirketAdi + " - " + x.Aciklama });
                MaliyetVM m = new MaliyetVM()
                {
                    Maliyet = _db.Maliyets.Find(id),
                    YapilanIss = _db.YapilanIss.ToList(),
                };
                ViewBag.IsList = new SelectList(iss, "id", "Name");
                return View(m);
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
                return View();
            }



        }
        [HttpPost]
        public ActionResult UpdateMal(MaliyetVM m, int id)
        {
            try
            {
                decimal a = 0;
                Maliyet z = _db.Maliyets.Find(m.Maliyet.ID);
                if (m.Maliyet.ToplamMaliyet != 0)
                {
                    z.ToplamMaliyet = m.Maliyet.ToplamMaliyet;
                }

                z.Aciklama = m.Maliyet.Aciklama;
                if (id != 0)
                {
                    z.YapilanIs.Kar += z.ToplamMaliyet;
                    z.YapilanIsID = id;
                    z.YapilanIs = _db.YapilanIss.Find(id);
                    z.YapilanIs.Kar -= z.ToplamMaliyet;
                }
                if (m.Maliyet.MaliyetOdemes.Sum(x => Convert.ToDecimal(x.Odeme)) != 0)
                {
                    for (int i = 0; i < z.MaliyetOdemes.Count; i++)
                    {
                        if (m.Maliyet.MaliyetOdemes[i].Odeme != 0)
                        {
                            z.MaliyetOdemes[i].Odeme = m.Maliyet.MaliyetOdemes[i].Odeme;
                        }
                        a += z.MaliyetOdemes[i].Odeme;
                    }
                    z.KalanOdeme = z.ToplamMaliyet - a;
                }
                if (z.ToplamMaliyet == a)
                {
                    z.OdemeDurumu = OdemeDurumu.Odendi;
                }
                else if (a == 0)
                {
                    z.OdemeDurumu = OdemeDurumu.Odenmedi;
                }
                else
                {
                    z.OdemeDurumu = OdemeDurumu.YarımOdendi;
                }
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";

            }

            return RedirectToAction("MaliyetTablosu");
        }
        public ActionResult MDeleteOdeme(int id)
        {
            try
            {
                MaliyetOdeme zz = _db.MaliyetOdemes.Find(id);
                zz.Maliyet.KalanOdeme += zz.Odeme;
                if (0 < zz.Maliyet.KalanOdeme && zz.Maliyet.KalanOdeme < zz.Maliyet.ToplamMaliyet)
                {
                    zz.Maliyet.OdemeDurumu = OdemeDurumu.YarımOdendi;
                }
                else if (zz.Maliyet.KalanOdeme == zz.Maliyet.ToplamMaliyet)
                {
                    zz.Maliyet.OdemeDurumu = OdemeDurumu.Odenmedi;
                }
                _db.MaliyetOdemes.Remove(zz);
                _db.SaveChanges();
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }

            return RedirectToAction("MaliyetTablosu");
        }
        public ActionResult TahsilEt(int id)
        {
            try
            {
                Cek zz = _db.Ceks.Find(id);
                zz.TahsilDurumu = TahsilDurumu.Alındı;
                _db.SaveChanges();
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }

            return RedirectToAction("Cek");
        }

        //Ana Sayfa Section ---------------------------------------------------------------

        public ActionResult AnaSayfa()
        {
            AnaSayfaVM asv = new AnaSayfaVM();
            try
            {
                
                kk = DateTime.Now;
                foreach (Cek item in _db.Ceks.Where(x => x.TahsilDurumu != TahsilDurumu.Alındı).ToList())
                {
                    CekTarih g = new CekTarih();
                    g.Cek = item;
                    TimeSpan z = item.TahsilatTarihi - kk;
                    int a = Convert.ToInt32(z.TotalDays);
                    g.Gun = a;
                    asv.CekTarihs.Add(g);
                }
                
            }
            catch (Exception e)
            {

                ViewBag.Message = $"Bir Hata İle Karşılaşıldı. Hata Kodu: {e}";
            }

            return View(asv);
        }
    }
}