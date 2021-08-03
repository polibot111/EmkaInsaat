using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.VMClasses
{
    public class AddWorkProjectVM
    {
        public List<WorkProject> WorkProjects { get; set; }

        public WorkProject WorkProject { get; set; }

        public List<Category> Categories { get; set; }

        public List<HttpPostedFileBase> httpPostedFileBases { get; set; }

        public AddWorkProjectVM()
        {
            httpPostedFileBases = new List<HttpPostedFileBase>();
        }
    }
}