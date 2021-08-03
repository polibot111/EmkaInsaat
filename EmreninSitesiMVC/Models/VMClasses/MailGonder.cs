using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.VMClasses
{
    public class MailGonder
    {

        public MailGonder()
        {
            Number = "Belirtilmedi.";
        }
        [Required(ErrorMessage = "{0} alanı bos gecilemez."), Display(Name = "Tam Ad")]
        public string FullName { get; set; }

        public string Number { get; set; }

        [Required(ErrorMessage = "{0} alanı bos gecilemez."),Display(Name ="Mail")]
        public string Maili { get; set; }

        [Required(ErrorMessage = "{0} alanı bos gecilemez."), Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}