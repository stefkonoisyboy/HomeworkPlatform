using Core.Dtos.Homework;

namespace Core.Interfaces
{
    public interface IHomeworkService
    {
        Task<HomeworkDto> CreateAsync(CreateHomeworkDto createHomeworkDto, string creatorId);

        Task<HomeworkDto> UpdateAsync(EditHomeworkDto editHomeworkDto);

        Task DeleteAsync(string homeworkId);

        Task<bool> ExistsAsync(string homeworkId);

        Task<HomeworkDto> GetByIdAsync(string homeworkId);

        Task<ICollection<HomeworkDto>> GetAllByClassIdAsync(string classId);
    }
}
