using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.VMClasses
{
    public class ImagePathVM
    {
        public ImagePath ImagePath { get; set; }

        public List<ImagePath> ImagePaths { get; set; }
    }
}