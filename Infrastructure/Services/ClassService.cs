using Core.Dtos.Class;
using Core.Dtos.User;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ClassService : IClassService
    {
        private readonly ApplicationContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public ClassService(
            ApplicationContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task AssignStudentAsync(string classId, string studentId)
        {
            UserClass userClass = new UserClass
            {
                ClassId = classId,
                StudentId = studentId,
            };

            await this.dbContext.UserClasses.AddAsync(userClass);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task AssignTeacherAsync(string classId, string teacherId)
        {
            UserClass userClass = new UserClass
            {
                ClassId = classId,
                StudentId = teacherId,
            };

            await this.dbContext.UserClasses.AddAsync(userClass);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckIfStudentIsAssignedForClass(string classId, string studentId)
        {
            return await this.dbContext.UserClasses
                .AnyAsync(uc => uc.ClassId == classId && uc.StudentId == studentId);
        }

        public async Task<bool> CheckIfTeacherIsAssignedForClass(string classId, string teacherId)
        {
            Class classEntity = await this.dbContext.Classes
                .FirstOrDefaultAsync(c => c.Id == classId);

            return classEntity.TeacherId == teacherId
                || await this.dbContext.UserClasses
                .AnyAsync(uc => uc.ClassId == classId && uc.StudentId == teacherId);
        }

        public async Task<bool> CheckIfTeacherIsCreatorForClass(string classId, string teacherId)
        {
            Class classEntity = await this.dbContext.Classes
                .FirstOrDefaultAsync(c => c.Id == classId);

            return classEntity.TeacherId == teacherId;
        }

        public async Task<bool> ExistsAsync(string classId)
        {
            return await this.dbContext.Classes.AnyAsync(c => c.Id == classId);
        }

        public async Task<ICollection<ClassDto>> GetAllAsync()
        {
            ICollection<ClassDto> classDtos = await this.dbContext.Classes
                .Select(c => new ClassDto
                {
                    Id = c.Id,
                    LogoUri = c.LogoUri,
                    Name = c.Name,
                    TeacherId = c.TeacherId,
                })
                .ToListAsync();

            return classDtos;
        }

        public async Task<ICollection<UserDto>> GetAllStudentsByClassIdAsync(string classId)
        {
            ICollection<ApplicationUser> students = await this.userManager
                .GetUsersInRoleAsync(Constants.STUDENT_ROLE);

            ICollection<UserDto> userDtos = await this.dbContext.Users
                .Include(u => u.EnrolledClasses)
                .Where(u => u.EnrolledClasses.Any(ec => ec.ClassId == classId))
                .Select(u => new UserDto
                {
                    Email = u.Email,
                    FirstName = u.FirstName,
                    Id = u.Id,
                    LastName = u.LastName,
                    UserName = u.UserName,
                })
                .ToListAsync();

            ICollection<UserDto> result = userDtos
                .Where(u => students.Any(s => s.Id == u.Id))
                .ToList();

            return result;
        }

        public async Task<ICollection<UserDto>> GetAllTeachersByClassIdAsync(string classId)
        {
            ICollection<ApplicationUser> teachers = await this.userManager
               .GetUsersInRoleAsync(Constants.TEACHER_ROLE);

            ICollection<UserDto> userDtos = await this.dbContext.Users
                .Include(u => u.EnrolledClasses)
                .Where(u => u.EnrolledClasses.Any(ec => ec.ClassId == classId))
                .Select(u => new UserDto
                {
                    Email = u.Email,
                    FirstName = u.FirstName,
                    Id = u.Id,
                    LastName = u.LastName,
                    UserName = u.UserName,
                })
                .ToListAsync();

            IList<UserDto> result = userDtos
               .Where(u => teachers.Any(s => s.Id == u.Id))
               .ToList();

            Class classEntity = await this.dbContext.Classes
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == classId);

            UserDto teacher = new UserDto
            {
                Id = classEntity.TeacherId,
                Email = classEntity.Teacher.Email,
                FirstName = classEntity.Teacher.FirstName,
                LastName = classEntity.Teacher.LastName,
                UserName = classEntity.Teacher.UserName,
            };

            result.Add(teacher);

            return result;
        }

        public async Task<ClassDto> GetByIdAsync(string classId)
        {
            ClassDto classDto = await this.dbContext.Classes
                .Where(c => c.Id == classId)
               .Select(c => new ClassDto
               {
                   Id = c.Id,
                   LogoUri = c.LogoUri,
                   Name = c.Name,
                   TeacherId = c.TeacherId,
               })
               .FirstOrDefaultAsync();

            return classDto;
        }

        public async Task RemoveStudentAsync(string classId, string studentId)
        {
            UserClass userClass = await this.dbContext.UserClasses
                .FirstOrDefaultAsync(uc => uc.ClassId == classId && uc.StudentId == studentId);

            this.dbContext.UserClasses.Remove(userClass);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveTeacherAsync(string classId, string teacherId)
        {
            UserClass userClass = await this.dbContext.UserClasses
                .FirstOrDefaultAsync(uc => uc.ClassId == classId && uc.StudentId == teacherId);

            this.dbContext.UserClasses.Remove(userClass);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
