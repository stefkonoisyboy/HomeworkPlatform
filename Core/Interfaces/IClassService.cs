namespace Core.Interfaces
{
    public interface IClassService
    {
        Task<bool> ExistsAsync(string classId);
    }
}
