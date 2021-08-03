using EmreninSitesiMVC.Models.Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public class Cek : BaseEntity
    {
        public string Sirket { get; set; }
        public string CekNo { get; set; }
        public DateTime AlisTarihi { get; set; }
        public DateTime TahsilatTarihi { get; set; }
        public decimal Tutar { get; set; }
        public TahsilDurumu TahsilDurumu { get; set; }
        public int YapilanIsID { get; set; }
        public CekSenet CekSenet { get; set; }

        //R.L
        public virtual YapilanIs YapilanIs { get; set; }
        public Cek()
        {
            AlisTarihi = DateTime.Now;
            TahsilatTarihi = DateTime.Now;
            //YapilanIs.SirketAdi = Sirket;
            CekSenet = CekSenet.Cek;
        }

    }
}