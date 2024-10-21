using Core.Dtos.Homework;
using Core.Dtos.HomeworkSubmission;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services
{
    public class HomeworkSubmissionService : IHomeworkSubmissionService
    {
        private readonly ApplicationContext dbContext;
        private readonly ICloudinaryService cloudinaryService;

        public HomeworkSubmissionService(
            ApplicationContext dbContext,
            ICloudinaryService cloudinaryService)
        {
            this.dbContext = dbContext;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<HomeworkSubmissionBasicDto> CreateAsync(CreateHomeworkSubmissionDto createHomeworkSubmissionDto, string studentId)
        {
            HomeworkSubmission homeworkSubmission = new HomeworkSubmission
            {
                StudentId = studentId,
                HomeworkId = createHomeworkSubmissionDto.HomeworkId,
            };

            EntityEntry<HomeworkSubmission> createdHomeworkSubmissionEntity = await this.dbContext.HomeworkSubmissions
                .AddAsync(homeworkSubmission);

            await this.dbContext.SaveChangesAsync();

            HomeworkSubmissionBasicDto homeworkSubmissionBasicDto = new HomeworkSubmissionBasicDto
            {
                HomeworkId = createdHomeworkSubmissionEntity.Entity.HomeworkId,
                StudentId = createdHomeworkSubmissionEntity.Entity.StudentId,
            };

            if (createHomeworkSubmissionDto.Attachments != null)
            {
                foreach (var file in createHomeworkSubmissionDto.Attachments)
                {
                    string attachmentUri = await this.cloudinaryService.UploadAsync(file);

                    Attachment attachment = new Attachment
                    {
                        AttachmentUri = attachmentUri,
                        CreatorId = studentId,
                        HomeworkSubmissionId = createdHomeworkSubmissionEntity.Entity.Id,
                    };

                    await this.dbContext.Attachments.AddAsync(attachment);
                }
            }

            return homeworkSubmissionBasicDto;
        }

        public async Task<bool> ExistsAsync(string homeworkSubmissionId)
        {
            return await this.dbContext.HomeworkSubmissions
                .AnyAsync(hs => hs.Id == homeworkSubmissionId);
        }

        public async Task<ICollection<HomeworkSubmissionDto>> GetAllByClassIdAndStudentIdAsync(string classId, string studentId)
        {
            ICollection<HomeworkSubmissionDto> homeworkSubmissionDtos = await this.dbContext.HomeworkSubmissions
                .Include(hs => hs.Homework)
                .Include(hs => hs.Student)
                .Include(hs => hs.Grade)
                .Where(hs => hs.Homework.ClassId == classId && hs.StudentId == studentId)
                .Select(hs => new HomeworkSubmissionDto
                {
                    Id = hs.Id,
                    StudentId = hs.StudentId,
                    CreatedAt = hs.CreatedAt,
                    GradePoints = hs.Grade.Points,
                    HomeworkInstructions = hs.Homework.Instructions,
                    HomeworkPoints = hs.Homework.Points,
                    HomeworkTitle = hs.Homework.Title,
                    Status = hs.Status.ToString(),
                    StudentFullName = hs.Student.FirstName + " " + hs.Student.LastName,
                })
                .ToListAsync();

            return homeworkSubmissionDtos;
        }

        public async Task<ICollection<HomeworkSubmissionDto>> GetAllByHomeworkIdAsync(string homeworkId)
        {
            ICollection<HomeworkSubmissionDto> homeworkSubmissionDtos = await this.dbContext.HomeworkSubmissions
                .Include(hs => hs.Homework)
                .Include(hs => hs.Student)
                .Include(hs => hs.Grade)
                .Where(hs => hs.HomeworkId == homeworkId)
                .Select(hs => new HomeworkSubmissionDto
                {
                    Id = hs.Id,
                    StudentId = hs.StudentId,
                    CreatedAt = hs.CreatedAt,
                    GradePoints = hs.Grade.Points,
                    HomeworkInstructions = hs.Homework.Instructions,
                    HomeworkPoints = hs.Homework.Points,
                    HomeworkTitle = hs.Homework.Title,
                    Status = hs.Status.ToString(),
                    StudentFullName = hs.Student.FirstName + " " + hs.Student.LastName,
                })
                .ToListAsync();

            return homeworkSubmissionDtos;
        }

        public async Task<HomeworkSubmissionDto> GetByIdAsync(string homeworkSubmissionId)
        {
            HomeworkSubmissionDto homeworkSubmissionDto = await this.dbContext.HomeworkSubmissions
                .Include(hs => hs.Homework)
                .Include(hs => hs.Student)
                .Include(hs => hs.Grade)
                .Where(hs => hs.Id == homeworkSubmissionId)
                .Select(hs => new HomeworkSubmissionDto
                {
                    Id = hs.Id,
                    StudentId = hs.StudentId,
                    CreatedAt = hs.CreatedAt,
                    GradePoints = hs.Grade.Points,
                    HomeworkInstructions = hs.Homework.Instructions,
                    HomeworkPoints = hs.Homework.Points,
                    HomeworkTitle = hs.Homework.Title,
                    Status = hs.Status.ToString(),
                    StudentFullName = hs.Student.FirstName + " " + hs.Student.LastName,
                })
                .FirstOrDefaultAsync();

            return homeworkSubmissionDto;
        }
    }
}
