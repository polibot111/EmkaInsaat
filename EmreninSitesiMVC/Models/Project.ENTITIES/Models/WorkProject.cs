using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Project.COMMON.Tools;


namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public class WorkProject : BaseEntity
    {
        [Required(ErrorMessage ="{0} alanı bos gecilemez."), Display(Name = "Firma Adı")]
        public string ProjectHead { get; set; }

        [Required(ErrorMessage = "{0} alanı bos gecilemez."), Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} alanı bos gecilemez."), Display(Name = "Firma Fiyat")]
        public string ProcetPrice { get; set; }

        [Required(ErrorMessage = "{0} alanı bos gecilemez."), Display(Name = "Teslim Süresi")]
        public string Time { get; set; }

        public int? CategoryID { get; set; }

        //R.L
        public virtual Category Category { get; set; }

        public virtual List<ImagePath> ImagePaths { get; set; }

        //public virtual ImagePath WallPaper { get; set; }

        public string WallPaper { get; set; }

        public WorkProject()
        {
            ImagePaths = new List<ImagePath>();
            WallPaper = "/OuterTools/Images/Form/NoImages.jpeg";
        }
    }
}