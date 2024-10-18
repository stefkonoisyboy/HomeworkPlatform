using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ClassService : IClassService
    {
        private readonly ApplicationContext dbContext;

        public ClassService(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(string classId)
        {
            return await this.dbContext.Classes.AnyAsync(c => c.Id == classId);
        }
    }
}
