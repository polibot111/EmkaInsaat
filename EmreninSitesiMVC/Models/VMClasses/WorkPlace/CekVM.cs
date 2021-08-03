using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.VMClasses.WorkPlace
{
    public class CekVM
    {
        public Cek Cek { get; set; }
        public List<Cek> Ceks { get; set; }
        public List<YapilanIs> YapilanIss { get; set; }
        public string IsNumarası { get; set; }
    }
}