using EmreninSitesiMVC.Models.Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public class EABorc:BaseEntity
    {
        public string Kime { get; set; }
        public string Aciklama { get; set; }
        public decimal? Borc { get; set; }
        public BorcTuru BorcTuru { get; set; }
        public OdemeDurumu OdemeDurumu { get; set; }

        //R.L
        public virtual List<EABorcOdeme> EABorcOdemes { get; set; }

        //public int SirketID { get; set; }

        ////R.L.
        //public virtual Sirket Sirket { get; set; }
        public EABorc()
        {
            BorcTuru = BorcTuru.Diger;
            OdemeDurumu = OdemeDurumu.Odenmedi;
            EABorcOdemes = new List<EABorcOdeme>();
        }

    }
}