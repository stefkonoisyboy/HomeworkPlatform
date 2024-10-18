using Core.Dtos.Homework;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly ApplicationContext dbContext;
        private readonly ICloudinaryService cloudinaryService;

        public HomeworkService(
            ApplicationContext dbContext,
            ICloudinaryService cloudinaryService)
        {
            this.dbContext = dbContext;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<HomeworkDto> CreateAsync(CreateHomeworkDto createHomeworkDto, string creatorId)
        {
            Homework homework = new Homework
            {
                Title = createHomeworkDto.Title,
                Instructions = createHomeworkDto.Instructions,
                Points = createHomeworkDto.Points,
                EndDate = createHomeworkDto.EndDate,
                ClassId = createHomeworkDto.ClassId,
            };

            EntityEntry<Homework> createdHomeworkEntity = await this.dbContext.Homeworks.AddAsync(homework);
            await this.dbContext.SaveChangesAsync();

            HomeworkDto homeworkDto = new HomeworkDto
            {
                Id = createdHomeworkEntity.Entity.Id,
                Title = createdHomeworkEntity.Entity.Title,
                Instructions = createdHomeworkEntity.Entity.Instructions,
                Points = createdHomeworkEntity.Entity.Points,
                EndDate = createdHomeworkEntity.Entity.EndDate,
            };

            foreach (var file in createHomeworkDto.Attachments)
            {
                string attachmentUri = await this.cloudinaryService.UploadAsync(file);

                Attachment attachment = new Attachment
                {
                    AttachmentUri = attachmentUri,
                    CreatorId = creatorId,
                    HomeworkId = createdHomeworkEntity.Entity.Id,
                };

                await this.dbContext.Attachments.AddAsync(attachment);
            }

            await this.dbContext.SaveChangesAsync();

            return homeworkDto;
        }

        public async Task DeleteAsync(string homeworkId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(string homeworkId)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<HomeworkDto>> GetAllByClassIdAsync(string classId)
        {
            throw new NotImplementedException();
        }

        public async Task<HomeworkDto> GetByIdAsync(string homeworkId)
        {
            throw new NotImplementedException();
        }

        public Task<HomeworkDto> UpdateAsync(EditHomeworkDto editHomeworkDto, string creatorId)
        {
            throw new NotImplementedException();
        }
    }
}
