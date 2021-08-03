using EmreninSitesiMVC.Models.Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.ENTITIES.Models
{
    public abstract class BaseEntity
    {
        public int ID { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Status Status { get; set; }
        public PageType PageType { get; set; }

        public BaseEntity()
        {
            Status = Status.Inserted;
            CreatedTime = DateTime.Now;
            PageType = PageType.WebSite;
        }
    }
}