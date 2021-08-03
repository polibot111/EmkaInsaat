using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using EmreninSitesiMVC.Models.VMClasses.ImagesTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.VMClasses
{
    public class WorkProjectVM
    {
        public List<WorkProject> WorkProjects { get; set; }

        public WorkProject WorkProject { get; set; }

        public List<Category> Categories { get; set; }
        public Category Category { get; set; }

        public AddImagesTool AddImagesTool { get; set; }

        public ImagePath ImagePath { get; set; }

        public List<ImagePath> ImagePaths { get; set; }

        public AppUser AppUser { get; set; }

    }
}