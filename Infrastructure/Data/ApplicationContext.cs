using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Class> Classes { get; set; }

        public DbSet<UserClass> UserClasses { get; set; }

        public DbSet<Homework> Homeworks { get; set; }

        public DbSet<HomeworkSubmission> HomeworkSubmissions { get; set; }

        public DbSet<Grade> Grades { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<UserReport> UserReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
