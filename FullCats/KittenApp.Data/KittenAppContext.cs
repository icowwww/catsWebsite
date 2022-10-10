
namespace KittenApp.Data
{
    using KittenApp.Models;
    using Microsoft.EntityFrameworkCore;

    public class KittenAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Kitten> Kittens { get; set; }

        public DbSet<Breed> Breeds { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "Data Source=DESKTOP-QSMGE05\\SQLEXPRESS;Database=KittenApp;Integrated Security=True";
               // Change first part to Server name. Example: string connectionString = "Data Source=DESKTOP-QSMGE05\\SQLEXPRESS;Database=KittenApp;Integrated Security=True";
                optionsBuilder.UseSqlServer(connectionString);

            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
