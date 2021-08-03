using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public class Sirket:BaseEntity
    {
        public decimal? ToplamGelir { get; set; }
        public decimal? ToplamGider { get; set; }
        //R.L
        public List<YapilanIs> YapilanIs { get; set; }
        public List<EABorc> EABorcs { get; set; }
    }
}