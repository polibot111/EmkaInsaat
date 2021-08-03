using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Xunit;
using Xunit.Sdk;

namespace EmreninSitesiMVC.Models.VMClasses.ImagesTool
{
    public class AddImagesTool
    {
        [Required(ErrorMessage = "Lütfen Dosya Seçiniz ")]
        [Display(Name = "Dosya Seç")]
        public List<HttpPostedFileBase> Files { get; set; }

        public AddImagesTool()
        {
            Files = new List<HttpPostedFileBase>();
        }
    }
}