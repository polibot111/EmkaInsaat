using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.VMClasses
{
    public class AppUserVM
    {
        public AppUser AppUser { get; set; }

        public string Password { get; set; }

        public string Control { get; set; }
    }
}