using EmreninSitesiMVC.AuthenticationClasses;
using EmreninSitesiMVC.Models.Project.DAL.Context;
using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using EmreninSitesiMVC.Models.VMClasses;
using Project.COMMON.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmreninSitesiMVC.Controllers
{
    [AdminAuthentication]
    public class PanelController : Controller
    {
        MyContext _db;

        public PanelController()
        {
            _db = new MyContext();
        }
        public ActionResult Index(int? id, string search)
        {

            if (search != null)
            {
                WorkProjectVM wpvm = new WorkProjectVM()
                {
                    Categories = _db.Categories.ToList(),
                    WorkProjects = id == null ? _db.WorkProjects.Where(x => x.Status != Models.Project.ENTITIES.Enums.Status.Deleted && x.ProjectHead.Contains(search)).ToList() : _db.WorkProjects.Where(x => x.CategoryID == id && x.Status != Models.Project.ENTITIES.Enums.Status.Deleted && x.ProjectHead.Contains(search)).ToList(),
                    AppUser = _db.AppUsers.FirstOrDefault(x => x.ID == 1),
                };

                return View(wpvm);
            }
            else
            {
                WorkProjectVM wpvm = new WorkProjectVM()
                {
                    Categories = _db.Categories.ToList(),
                    WorkProjects = id == null ? _db.WorkProjects.Where(x => x.Status != Models.Project.ENTITIES.Enums.Status.Deleted).ToList() : _db.WorkProjects.Where(x => x.CategoryID == id && x.Status != Models.Project.ENTITIES.Enums.Status.Deleted).ToList(),
                    AppUser = _db.AppUsers.FirstOrDefault(x => x.ID == 1),
                };

                return View(wpvm);
            }

        }
        //[HttpPost]
        //public ActionResult Index(int id, string head)
        //{
        //    if (head != null)
        //    {
        //        WorkProjectVM wpvm = new WorkProjectVM()
        //        {
        //            //_db.WorkProjects.Where(x=>x.ProjectHead.Contains(item)).ToList(),
        //            WorkProjects = _db.WorkProjects.Where(x => x.CategoryID == id && x.Status != Models.Project.ENTITIES.Enums.Status.Deleted && x.ProjectHead.Contains(head)).ToList(),
        //        };
        //        return View(wpvm);
        //    }
        //    else
        //    {
        //        WorkProjectVM wpvm = new WorkProjectVM()
        //        {
        //            WorkProjects = _db.WorkProjects.Where(x => x.CategoryID == id && x.Status != Models.Project.ENTITIES.Enums.Status.Deleted).ToList(),
        //        };
        //        return View(wpvm);
        //    }

        //}

        public ActionResult DeleteIndex(int? id, string search)
        {
            if (search != null)
            {
                WorkProjectVM wpvm = new WorkProjectVM()
                {
                    Categories = _db.Categories.ToList(),
                    WorkProjects = id == null ? _db.WorkProjects.Where(x => x.Status == Models.Project.ENTITIES.Enums.Status.Deleted && x.ProjectHead.Contains(search)).ToList() : _db.WorkProjects.Where(x => x.CategoryID == id && x.Status == Models.Project.ENTITIES.Enums.Status.Deleted && x.ProjectHead.Contains(search)).ToList(),
                };

                return View(wpvm);
            }
            else
            {
                WorkProjectVM wpvm = new WorkProjectVM()
                {
                    Categories = _db.Categories.ToList(),
                    WorkProjects = id == null ? _db.WorkProjects.Where(x => x.Status == Models.Project.ENTITIES.Enums.Status.Deleted).ToList() : _db.WorkProjects.Where(x => x.CategoryID == id && x.Status == Models.Project.ENTITIES.Enums.Status.Deleted).ToList(),
                };

                return View(wpvm);
            }
        }

        public ActionResult LogOut()
        {
            Session.Remove("admin");
            return RedirectToAction("Login", "Admin");

        }

        public ActionResult AddNewProject()
        {
            WorkProjectVM wpvm = new WorkProjectVM()
            {
                WorkProject = new WorkProject(),
                Categories = _db.Categories.Where(x => x.Status != Models.Project.ENTITIES.Enums.Status.Deleted).ToList(),
            };
            return View(wpvm);
        }
        [HttpPost]
        public ActionResult AddNewProject(WorkProjectVM wproject, IEnumerable<HttpPostedFileBase> resim)
        {
            _db.WorkProjects.Add(wproject.WorkProject);
            foreach (HttpPostedFileBase item in resim)
            {
                string[] fileArray = item.FileName.Split('.');
                string extension = fileArray[fileArray.Length - 1].ToLower();
                if (extension == "jpg" || extension == "gif" || extension == "png" || extension == "jpeg")
                {
                    ImagePath im = new ImagePath();
                    im.ImageUrl = ImageUploader.UploadImage("~/Images/CategoryImage/" + wproject.WorkProject.CategoryID + "/", item);
                    wproject.WorkProject.ImagePaths.Add(im);
                }
            }
            _db.SaveChanges();
            try
            {
                if (wproject.WorkProject.ImagePaths[0] != null)
                {
                    wproject.WorkProject.WallPaper = wproject.WorkProject.ImagePaths[0].ImageUrl;
                };
                _db.SaveChanges();
            }
            catch (Exception)
            {


            }


            return RedirectToAction("ImageEdit", new { id = wproject.WorkProject.ID });


        }

        public ActionResult ImageEdit(int id)
        {
            WorkProjectVM imvm = new WorkProjectVM()
            {
                WorkProjects = _db.WorkProjects.Where(x => x.ID == id).ToList(),
                ImagePaths = _db.ImagePaths.Where(x => x.WorkProjectID == id).ToList(),
            };
            return View(imvm);
        }

        [HttpPost]

        public ActionResult ImageEdit(WorkProject im)
        {

            WorkProject up = _db.WorkProjects.Find(im.ID);
            for (int i = 0; i < im.ImagePaths.Count; i++)
            {
                up.ImagePaths[i].Description = (im.ImagePaths[i].Description != null) ? im.ImagePaths[i].Description : up.ImagePaths[i].Description;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult UpdateWorkProject(int id)
        {
            WorkProjectVM wp = new WorkProjectVM
            {
                Categories = _db.Categories.Where(x => x.Status != Models.Project.ENTITIES.Enums.Status.Deleted).ToList(),
                WorkProject = _db.WorkProjects.Find(id),
                ImagePaths = _db.ImagePaths.Where(x => x.WorkProjectID == id).ToList(),
            };
            return View(wp);
        }

        [HttpPost]
        public ActionResult UpdateWorkProject(WorkProjectVM wp, IEnumerable<HttpPostedFileBase> resim)
        {

            WorkProject sa = _db.WorkProjects.Find(wp.WorkProject.ID);

            sa.Description = (wp.WorkProject.Description != null) ? wp.WorkProject.Description : sa.Description;
            sa.Time = (wp.WorkProject.Time != null) ? wp.WorkProject.Time : sa.Time;
            sa.ProjectHead = (wp.WorkProject.ProjectHead != null) ? wp.WorkProject.ProjectHead : sa.ProjectHead;
            sa.ProcetPrice = (wp.WorkProject.ProcetPrice != null) ? wp.WorkProject.ProcetPrice : sa.ProcetPrice;
            sa.CategoryID = (wp.WorkProject.CategoryID != null) ? wp.WorkProject.CategoryID : sa.CategoryID;

            foreach (HttpPostedFileBase control in resim)
            {
                if (control != null)
                {
                    string[] fileArray = control.FileName.Split('.');
                    string extension = fileArray[fileArray.Length - 1].ToLower();
                    if (extension == "jpg" || extension == "gif" || extension == "png" || extension == "jpeg")
                    {
                        foreach (HttpPostedFileBase item in resim)
                        {
                            ImagePath im = new ImagePath();
                            im.ImageUrl = ImageUploader.UploadImage("~/Images/CategoryImage/" + sa.CategoryID + "/", item);
                            sa.ImagePaths.Add(im);
                        }

                        break;
                    }


                }
                else
                {
                    break;
                }
            }
            sa.Status = Models.Project.ENTITIES.Enums.Status.Updated;
            _db.SaveChanges();
            return RedirectToAction("ImageEdit", new { id = sa.ID });

        }

        public ActionResult GetActive(int id)
        {
            WorkProject wp = _db.WorkProjects.Find(id);
            wp.Status = Models.Project.ENTITIES.Enums.Status.Updated;
            _db.SaveChanges();
            return RedirectToAction("DeleteIndex");
        }

        public ActionResult DeleteImage(int imdi, int wpid)
        {
            _db.ImagePaths.Remove(_db.ImagePaths.Find(imdi));
            _db.SaveChanges();
            return RedirectToAction("ImageEdit", "Panel", new { id = wpid });
        }

        public ActionResult PassiveProject(int id)
        {
            WorkProject wp = _db.WorkProjects.Find(id);
            wp.Status = Models.Project.ENTITIES.Enums.Status.Deleted;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteProject(int id)
        {
            _db.WorkProjects.Remove(_db.WorkProjects.Find(id));
            _db.SaveChanges();
            return RedirectToAction("DeleteIndex");
        }

        public ActionResult WallPaper(int wpid, string imdi)
        {
            _db.WorkProjects.Find(wpid).WallPaper = imdi;
            _db.SaveChanges();
            return RedirectToAction("ImageEdit", "Panel", new { id = wpid });
        }



    }
}