using EmreninSitesiMVC.Models.Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public class YapilanIs:BaseEntity
    {
        public string SirketAdi { get; set; }
        public string Temsilcisi { get; set; }
        public string Aciklama { get; set; }
        public OdemeSekli OdemeSekli  { get; set; }
        public string FaturaNo { get; set; }
        public decimal FaturaFiyat { get; set; }
        public string FaturaAcıklama { get; set; }
        public string HakedisNo { get; set; }
        public decimal HakedisFiyat { get; set; }
        public decimal? Kar { get; set; }
        public decimal? KalanOdeme { get; set; }
        public YapılacakIsler IsDurumu { get; set; }
        public OdemeDurumu OdemeDurumu { get; set; }
        public decimal TotalOdeme { get; set; }
        //R.L
        public virtual List<Maliyet> Maliyets { get; set; }
        public virtual List<Cek> Cek { get; set; }
        public virtual List<YapilanIsOdeme> YapilanIsOdemes { get; set; }
        public YapilanIs()
        {
            OdemeSekli = OdemeSekli.TL;
            Cek = new List<Cek>();
            Maliyets = new List<Maliyet>();
            YapilanIsOdemes = new List<YapilanIsOdeme>();
        }

    }
}