using EmreninSitesiMVC.Models.Project.DAL.StrategyPattern;
using EmreninSitesiMVC.Models.Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EmreninSitesiMVC.Models.Project.DAL.Context
{
    public class MyContext : DbContext
    {
        public MyContext() : base("myConnection")
        {
            Database.SetInitializer(new MyInit());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EABorcOdeme>().HasRequired(current => current.EABorc)
             .WithMany(c => c.EABorcOdemes)
             .HasForeignKey(c => c.EABorcID)
             .WillCascadeOnDelete(true);
            modelBuilder.Entity<YapilanIsOdeme>().HasRequired(z => z.YapilanIs).WithMany(c => c.YapilanIsOdemes).HasForeignKey(z => z.YapilanIsID).WillCascadeOnDelete(true);
            modelBuilder.Entity<MaliyetOdeme>().HasRequired(z => z.Maliyet).WithMany(c => c.MaliyetOdemes).HasForeignKey(z => z.MaliyetID).WillCascadeOnDelete(true);
            modelBuilder.Entity<Maliyet>().HasRequired(z => z.YapilanIs).WithMany(x => x.Maliyets).HasForeignKey(z => z.YapilanIsID).WillCascadeOnDelete(true);

            modelBuilder.Entity<ImagePath>().HasRequired(current => current.WorkProject)
            .WithMany(c => c.ImagePaths)
            .HasForeignKey(c => c.WorkProjectID)
            .WillCascadeOnDelete(true);
            modelBuilder.Entity<Cek>().HasRequired(current => current.YapilanIs)
            .WithMany(c => c.Cek)
            .HasForeignKey(c => c.YapilanIsID)
            .WillCascadeOnDelete(true);
            //İş tablosunda database ile alakalı sorun çıkarsa buraya dikkat et.
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ImagePath> ImagePaths { get; set; }
        public DbSet<WorkProject> WorkProjects { get; set; }
        public DbSet<Cek> Ceks { get; set; }
        public DbSet<Maliyet> Maliyets { get; set; }
        public DbSet<YapilanIs> YapilanIss { get; set; }
        public DbSet<EABorc> EABorcs { get; set; }
        public DbSet<Sirket> Sirkets { get; set; }
        public DbSet<EABorcOdeme> EABorcOdemes { get; set; }
        public DbSet<MaliyetOdeme> MaliyetOdemes { get; set; }
        public DbSet<YapilanIsOdeme> YapilanIsOdemes { get; set; }


    }
}