using Core.Dtos.User;

namespace Core.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> UpdateAsync(EditUserDto editUserDto);

        Task DeleteAsync(string userId);

        Task<bool> ExistsAsync(string userId);

        Task<UserDto> GetByIdAsync(string userId);
    }
}
