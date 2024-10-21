using Core.Dtos.User;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext dbContext;

        public UserService(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteAsync(string userId)
        {
            ApplicationUser user = await this.dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            this.dbContext.Users.Remove(user);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string userId)
        {
            return await this.dbContext.Users
                .AnyAsync(u => u.Id == userId);
        }

        public async Task<UserDto> GetByIdAsync(string userId)
        {
            UserDto userDto = await this.dbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                })
                .FirstOrDefaultAsync();

            return userDto;
        }

        public async Task<UserDto> UpdateAsync(EditUserDto editUserDto)
        {
            ApplicationUser user = await this.dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == editUserDto.Id);

            user.FirstName = editUserDto.FirstName;
            user.LastName = editUserDto.LastName;

            EntityEntry<ApplicationUser> updatedUserEntity = this.dbContext.Users.Update(user);
            await this.dbContext.SaveChangesAsync();

            UserDto userDto = new UserDto
            {
                Id = updatedUserEntity.Entity.Id,
                UserName = updatedUserEntity.Entity.UserName,
                Email = updatedUserEntity.Entity.Email,
                FirstName = updatedUserEntity.Entity.FirstName,
                LastName = updatedUserEntity.Entity.LastName,
            };

            return userDto;
        }
    }
}
