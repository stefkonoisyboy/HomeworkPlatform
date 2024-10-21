using Core.Dtos.HomeworkSubmission;

namespace Core.Interfaces
{
    public interface IHomeworkSubmissionService
    {
        Task<HomeworkSubmissionBasicDto> CreateAsync(CreateHomeworkSubmissionDto createHomeworkSubmissionDto, string studentId);

        Task<bool> ExistsAsync(string homeworkSubmissionId);

        Task<HomeworkSubmissionDto> GetByIdAsync(string homeworkSubmissionId);

        Task<ICollection<HomeworkSubmissionDto>> GetAllByClassIdAndStudentIdAsync(string classId, string studentId);

        Task<ICollection<HomeworkSubmissionDto>> GetAllByHomeworkIdAsync(string homeworkId);
    }
}
