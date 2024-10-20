using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext dbContext;

        public UserService(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(string userId)
        {
            return await this.dbContext.Users
                .AnyAsync(u => u.Id == userId);
        }
    }
}
