using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.VMClasses.WorkPlace
{
    public class AnaSayfaVM
    {
        public List<Cek> Ceks { get; set; }
        public List<YapilanIs> YapilanIss { get; set; }
        public List<EABorc> EABorcs { get; set; }
        public List<CekTarih> CekTarihs { get; set; }
        public AnaSayfaVM()
        {
            CekTarihs = new List<CekTarih>();
        }
    }
}