using Core.Dtos.Class;
using Core.Dtos.User;

namespace Core.Interfaces
{
    public interface IClassService
    {
        Task AssignStudentAsync(string classId,  string studentId);

        Task AssignTeacherAsync(string classId, string teacherId);

        Task RemoveStudentAsync(string classId, string studentId);

        Task RemoveTeacherAsync(string classId, string teacherId);
            
        Task<bool> ExistsAsync(string classId);

        Task<bool> CheckIfStudentIsAssignedForClass(string classId, string studentId);

        Task<bool> CheckIfTeacherIsAssignedForClass(string classId, string teacherId);

        Task<bool> CheckIfTeacherIsCreatorForClass(string classId, string teacherId);

        Task<ICollection<ClassDto>> GetAllAsync();

        Task<ClassDto> GetByIdAsync(string classId);

        Task<ICollection<UserDto>> GetAllStudentsByClassIdAsync(string classId);

        Task<ICollection<UserDto>> GetAllTeachersByClassIdAsync(string classId);
    }
}
