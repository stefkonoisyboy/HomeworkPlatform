using Core.Dtos.Grade;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services
{
    public class GradeService : IGradeService
    {
        private readonly ApplicationContext dbContext;

        public GradeService(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CheckExistingGradeAsync(string homeworkSubmissionId)
        {
            return await this.dbContext.Grades
                .AnyAsync(g => g.HomeworkSubmissionId == homeworkSubmissionId);
        }

        public async Task<bool> CheckIfGradeIsValidAsync(int points, string homeworkSubmissionId)
        {
            HomeworkSubmission homeworkSubmission = await this.dbContext.HomeworkSubmissions
               .Include(hs => hs.Homework)
               .FirstOrDefaultAsync(hs => hs.Id == homeworkSubmissionId);

            return points <= homeworkSubmission.Homework.Points.Value;
        }

        public async Task<bool> CheckIfHomeworkHasPointsAsync(string homeworkSubmissionId)
        {
            HomeworkSubmission homeworkSubmission = await this.dbContext.HomeworkSubmissions
                .Include(hs => hs.Homework)
                .FirstOrDefaultAsync(hs => hs.Id == homeworkSubmissionId);

            return homeworkSubmission.Homework.Points.HasValue;
        }

        public async Task<GradeDto> CreateAsync(CreateGradeDto createGradeDto)
        {
            Grade grade = new Grade
            {
                Points = createGradeDto.Points,
                Feedback = createGradeDto.Feedback,
                HomeworkSubmissionId = createGradeDto.HomeworkSubmissionId,
            };

            EntityEntry<Grade> createdGradeEntity = await this.dbContext.Grades.AddAsync(grade);
            await this.dbContext.SaveChangesAsync();

            HomeworkSubmission homeworkSubmission = await this.dbContext.HomeworkSubmissions
                .FirstOrDefaultAsync(hs => hs.Id == createGradeDto.HomeworkSubmissionId);

            homeworkSubmission.Status = Core.Entities.Enums.HomeworkSubmissionStatus.Graded;
            homeworkSubmission.GradeId = createdGradeEntity.Entity.Id;

            await this.dbContext.SaveChangesAsync();

            GradeDto gradeDto = new GradeDto
            {
                Id = createdGradeEntity.Entity.Id,
                Points = createdGradeEntity.Entity.Points,
                Feedback = createdGradeEntity.Entity.Feedback,
                HomeworkSubmissionId = createdGradeEntity.Entity.HomeworkSubmissionId
            };

            return gradeDto;
        }

        public async Task<bool> ExistsAsync(string gradeId)
        {
            return await this.dbContext.Grades
                .AnyAsync(g => g.Id == gradeId);
        }

        public async Task<GradeDto> GetByHomeworkSubmissionIdAsync(string homeworkSubmissionId)
        {
            GradeDto gradeDto = await this.dbContext.Grades
                .Where(g => g.HomeworkSubmissionId == homeworkSubmissionId)
                .Select(g => new GradeDto
                {
                    Id = g.Id,
                    Points = g.Points,
                    Feedback = g.Feedback,
                    HomeworkSubmissionId = g.HomeworkSubmissionId,
                })
                .FirstOrDefaultAsync();

            return gradeDto;
        }

        public async Task<GradeDto> UpdateAsync(EditGradeDto editGradeDto)
        {
            Grade grade = await this.dbContext.Grades
                .FirstOrDefaultAsync(g => g.Id == editGradeDto.Id);

            grade.Points = editGradeDto.Points;
            grade.Feedback = editGradeDto.Feedback;
            grade.HomeworkSubmissionId = editGradeDto.HomeworkSubmissionId;

            EntityEntry<Grade> updatedGradeEntity = this.dbContext.Grades.Update(grade);
            await this.dbContext.SaveChangesAsync();

            GradeDto gradeDto = new GradeDto
            {
                Id = updatedGradeEntity.Entity.Id,
                Points = updatedGradeEntity.Entity.Points,
                Feedback = updatedGradeEntity.Entity.Feedback,
                HomeworkSubmissionId = updatedGradeEntity.Entity.HomeworkSubmissionId,
            };

            return gradeDto;
        }
    }
}
