namespace Cafe_Management_System
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class CafeManagementSystemEntities : DbContext
    {
        public CafeManagementSystemEntities()
            : base("name=CafeManagementSystemEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Primary key for the 'user' entity
            modelBuilder.Entity<user>().HasKey(u => u.Username);

            // Define primary keys for other entities
            modelBuilder.Entity<DailySale>().HasKey(ds => ds.SaleID);
            modelBuilder.Entity<item>().HasKey(i => i.id);
            modelBuilder.Entity<sysdiagram>().HasKey(sd => sd.Id); // Assuming Id is the primary key

            // Add other configurations...

            // Call the base OnModelCreating to ensure that the default configurations are applied
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<DailySale> DailySales { get; set; }
        public virtual DbSet<item> items { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<user> users { get; set; }
    }

    public class DailySale
    {
        public int SaleID { get; set; }
    }

    public class item
    {
        public int id { get; set; }
    }

    public class user
    {
        public string Role { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }

    public class sysdiagram
    {
        public int Id { get; set; }
        // Add other properties as needed...
    }
}
