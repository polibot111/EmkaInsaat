using EmreninSitesiMVC.Models.Project.DAL.Context;
using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using Project.COMMON.Tools;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.DAL.StrategyPattern
{
    public class MyInit : CreateDatabaseIfNotExists<MyContext>
    {

        protected override void Seed(MyContext context)
        {
            #region Admin
            AppUser au = new AppUser();
            au.UserName = "admin";
            au.Password = DantexCrypt.Crypt("123emre");
            au.Email = "kchnfrkn@gmail.com";

            context.AppUsers.Add(au);

            context.SaveChanges();

            #endregion

            List<Category> categories = new List<Category>()
            {
                new Category {CategoryName="Muhendislik"},
                new Category {CategoryName="Mimarlik"},
                new Category {CategoryName="Muteahhitlik"}
            };
            foreach (Category item in categories)
            {
                context.Categories.Add(item);
            }

            
            context.SaveChanges();


              
        }
    }
}