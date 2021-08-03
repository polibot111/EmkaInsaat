using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.VMClasses.WorkPlace
{
    public class MaliyetVM
    {
        public List<Maliyet> Maliyets{ get; set; }
        public Maliyet Maliyet{ get; set; }
        public List<YapilanIs> YapilanIss{ get; set; }
        public YapilanIs YapilanIs { get; set; }
    }
}