using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Bot.model;

namespace Bot.Utilities
{
    public class BotdbUtil : DbContext
    {
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerMessage> CustomerMessage { get; set; }
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
            modelBuilder.Entity<CustomerMessage>()
                .HasKey(m => m.Id).Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<CustomerServer>()
               .HasKey(m => m.UserId);
            base.OnModelCreating(modelBuilder);
        }
    }
}