using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public class MaliyetOdeme:BaseEntity
    {
        public decimal Odeme { get; set; }
        public DateTime Tarih { get; set; }
        public int MaliyetID { get; set; }

        //R.L
        public virtual Maliyet Maliyet { get; set; }
        public MaliyetOdeme()
        {
            Tarih = DateTime.Now;
        }
    }
}