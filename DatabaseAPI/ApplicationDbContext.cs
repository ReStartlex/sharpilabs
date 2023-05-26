using DatabaseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAPI
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<DBUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<DBUser>(dbUser =>
            {
                dbUser.ToTable("users");
                dbUser.HasKey(user => user.Id);
                dbUser.Property(user => user.Id).HasColumnName("id");
                dbUser.Property(user => user.Login).HasColumnName("login");
                dbUser.Property(user => user.Password).HasColumnName("password");
                dbUser.Property(user => user.Name).HasColumnName("name");
            });
        }
    }
}
