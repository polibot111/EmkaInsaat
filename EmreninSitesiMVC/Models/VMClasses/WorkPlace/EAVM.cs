
using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.VMClasses.WorkPlace
{
    public class EAVM
    {
        public EABorc SingleEA { get; set; }
        public List<EABorc> ListEA { get; set; }
        public EABorcOdeme SingleOdeme { get; set; }
        public List<EABorcOdeme> ListOdeme { get; set; }
    }
}