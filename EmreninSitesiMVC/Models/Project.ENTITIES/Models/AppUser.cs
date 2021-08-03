using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public class AppUser:BaseEntity
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public Guid ActivationCode { get; set; }

        public Guid EmailChange { get; set; }

        public bool Active { get; set; }

        public string Control { get; set; }

    }
}