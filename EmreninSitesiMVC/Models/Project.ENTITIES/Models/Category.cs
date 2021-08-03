using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; }

        //R.L.
        public virtual List<WorkProject> Projects { get; set; }

        public Category()
        {
            Projects = new List<WorkProject>();
        }
    }
}