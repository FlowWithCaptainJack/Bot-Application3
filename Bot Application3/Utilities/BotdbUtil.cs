using System.Data.Entity;
using Bot.model;

namespace Bot_Application3.Utilities
{
    public class BotdbUtil : DbContext
    {
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerServer> CustomerServer { get; set; }
        public BotdbUtil() : base("botdb")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .HasKey(m => m.UserId);
            modelBuilder.Entity<Customer>()
                .HasKey(m => m.UserId);
            modelBuilder.Entity<CustomerServer>()
               .HasKey(m => m.UserId);
        }
    }
}