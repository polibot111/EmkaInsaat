using EmreninSitesiMVC.Models.Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public class Maliyet:BaseEntity
    {
        public string Aciklama { get; set; }
        public decimal ToplamMaliyet { get; set; }
        public decimal KalanOdeme { get; set; }
        public int? YapilanIsID { get; set; }
        public OdemeDurumu OdemeDurumu { get; set; }

        //R.L
        public virtual YapilanIs YapilanIs { get; set; }
        public virtual List<MaliyetOdeme> MaliyetOdemes { get; set; }

        public Maliyet()
        {

            MaliyetOdemes = new List<MaliyetOdeme>();
        }
    }
}