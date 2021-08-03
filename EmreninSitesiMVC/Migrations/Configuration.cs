namespace EmreninSitesiMVC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EmreninSitesiMVC.Models.Project.DAL.Context.MyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "EmreninSitesiMVC.Models.Project.DAL.Context.MyContext";
        }

        protected override void Seed(EmreninSitesiMVC.Models.Project.DAL.Context.MyContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
