using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public class YapilanIsOdeme:BaseEntity
    {
        public decimal Odeme { get; set; }
        public DateTime Tarih { get; set; }

        public int YapilanIsID { get; set; }
        //R.L

        public virtual YapilanIs YapilanIs { get; set; }

        public YapilanIsOdeme()
        {
            Tarih = DateTime.Now;
        }
    }
}