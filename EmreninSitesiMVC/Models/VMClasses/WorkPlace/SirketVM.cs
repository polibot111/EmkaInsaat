using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.VMClasses.WorkPlace
{
    public class SirketVM
    {
        public YapilanIs YapilanIs { get; set; }
        public List<YapilanIs> YapilanIss { get; set; }
        public List<Cek> Cek { get; set; }

    }
}