using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public class ImagePath:BaseEntity
    {
        public string ImageUrl { get; set; }

        public int WorkProjectID { get; set; }

        public string Description { get; set; }

        //R.L

        public WorkProject WorkProject { get; set; }
    }
}