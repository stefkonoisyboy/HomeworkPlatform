using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationContextSeed
    {
        public static async Task SeedAsync(ApplicationContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await context.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = Constants.ADMINISTRATOR_ROLE,
                });

                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = Constants.TEACHER_ROLE,
                });

                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = Constants.STUDENT_ROLE,
                });
            }

            if (!await context.Users.AnyAsync())
            {
                ApplicationUser admin = new ApplicationUser
                {
                    Email = "admin@abv.bg",
                    UserName = "admin@abv.bg",
                    FirstName = "Admin",
                    LastName = "Adminov"
                };

                ApplicationUser teacher = new ApplicationUser
                {
                    Email = "teacher@abv.bg",
                    UserName = "teacher@abv.bg",
                    FirstName = "Teacher",
                    LastName = "Teacherov"
                };

                ApplicationUser student = new ApplicationUser
                {
                    Email = "student@abv.bg",
                    UserName = "student@abv.bg",
                    FirstName = "Student",
                    LastName = "Studentov"
                };

                string password = "Pa$$w0rd";

                await userManager.CreateAsync(admin, password);
                await userManager.CreateAsync(teacher, password);
                await userManager.CreateAsync(student, password);

                await userManager.AddToRoleAsync(admin, Constants.ADMINISTRATOR_ROLE);
                await userManager.AddToRoleAsync(teacher, Constants.TEACHER_ROLE);
                await userManager.AddToRoleAsync(student, Constants.STUDENT_ROLE);

                if (!await context.Classes.AnyAsync())
                {
                    ICollection<Class> classes = new List<Class>()
                    {
                        new Class
                        {
                            Name = "Class 1",
                            TeacherId = teacher.Id,
                        },
                        new Class
                        {
                            Name = "Class 2",
                            TeacherId = teacher.Id,
                        }
                    };

                    await context.Classes.AddRangeAsync(classes);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
