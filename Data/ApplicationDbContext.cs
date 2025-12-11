using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        //public DbSet<Certificate> Certificates { get; set; }
        //public DbSet<Comment> Comments { get; set; }
        //public DbSet<Course> Courses { get; set; }
    }
}
