using Core.Dtos.Grade;

namespace Core.Interfaces
{
    public interface IGradeService
    {
        Task<GradeDto> CreateAsync(CreateGradeDto createGradeDto);

        Task<GradeDto> UpdateAsync(EditGradeDto editGradeDto);

        Task<bool> ExistsAsync(string gradeId);

        Task<bool> CheckExistingGradeAsync(string homeworkSubmissionId);

        Task<bool> CheckIfHomeworkHasPointsAsync(string homeworkSubmissionId);

        Task<bool> CheckIfGradeIsValidAsync(int points, string homeworkSubmissionId);

        Task<GradeDto> GetByHomeworkSubmissionIdAsync(string homeworkSubmissionId);
    }
}
