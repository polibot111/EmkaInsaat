using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public class EABorcOdeme:BaseEntity
    {
        public decimal Odeme { get; set; }
        public DateTime Tarih { get; set; }

        public int EABorcID { get; set; }
        //R.L

        public virtual EABorc EABorc { get; set; }
        public EABorcOdeme()
        {
            Tarih = DateTime.Now;
        }
    }
}