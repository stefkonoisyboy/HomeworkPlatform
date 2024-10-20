namespace Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> ExistsAsync(string userId);
    }
}
