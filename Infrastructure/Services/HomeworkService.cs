using Core.Dtos.Homework;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
            DateTime endDate = DateTime.UtcNow;
            bool isDateConversionSuccessful = false;

            if (createHomeworkDto.EndDate != null)
            {
                isDateConversionSuccessful = DateTime.TryParse(createHomeworkDto.EndDate, out endDate);
            }

            Homework homework = new Homework
            {
                Title = createHomeworkDto.Title,
                Instructions = createHomeworkDto.Instructions,
                Points = createHomeworkDto.Points,
                EndDate = isDateConversionSuccessful ? endDate : null,
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
                ClassId = createdHomeworkEntity.Entity.ClassId,
            };

            if (createHomeworkDto.Attachments != null)
            {
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
            }

            await this.dbContext.SaveChangesAsync();

            return homeworkDto;
        }

        public async Task DeleteAsync(string homeworkId)
        {
            ICollection<Attachment> attachments = await this.dbContext.Attachments
                .Where(a => a.HomeworkId == homeworkId)
                .ToListAsync();

            this.dbContext.Attachments.RemoveRange(attachments);

            Homework homework = await this.dbContext.Homeworks
                .FirstOrDefaultAsync(h => h.Id == homeworkId);

            this.dbContext.Homeworks.Remove(homework);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string homeworkId)
        {
            return await this.dbContext.Homeworks
                .AnyAsync(h => h.Id == homeworkId);
        }

        public async Task<ICollection<HomeworkDto>> GetAllByClassIdAsync(string classId)
        {
            ICollection<HomeworkDto> homeworkDtos = await this.dbContext.Homeworks
                .Where(h => h.ClassId == classId)
                .Select(h => new HomeworkDto
                {
                    Id = h.Id,
                    Title = h.Title,
                    EndDate = h.EndDate,
                    Instructions = h.Instructions,
                    Points = h.Points,
                    ClassId = h.ClassId,
                })
                .ToListAsync();

            return homeworkDtos;
        }

        public async Task<HomeworkDto> GetByIdAsync(string homeworkId)
        {
            Homework homework = await this.dbContext.Homeworks
                .FirstOrDefaultAsync(h => h.Id == homeworkId);

            HomeworkDto homeworkDto = new HomeworkDto
            {
                Id = homework.Id,
                Title = homework.Title,
                Instructions = homework.Instructions,
                Points = homework.Points,
                EndDate = homework.EndDate,
                ClassId = homework.ClassId,
            };

            return homeworkDto;
        }

        public async Task<HomeworkDto> UpdateAsync(EditHomeworkDto editHomeworkDto)
        {
            Homework homework = await this.dbContext.Homeworks
                .FirstOrDefaultAsync(h => h.Id ==  editHomeworkDto.Id);

            DateTime endDate = DateTime.UtcNow;
            bool isDateConversionSuccessful = false;

            if (editHomeworkDto.EndDate != null)
            {
                isDateConversionSuccessful = DateTime.TryParse(editHomeworkDto.EndDate, out endDate);
            }

            homework.Title = editHomeworkDto.Title;
            homework.Instructions = editHomeworkDto.Instructions;
            homework.Points = editHomeworkDto.Points;
            homework.EndDate = isDateConversionSuccessful ? endDate : homework.EndDate;
            homework.ClassId = editHomeworkDto.ClassId;

            EntityEntry<Homework> updatedHomeworkEntity = this.dbContext.Homeworks.Update(homework);
            await this.dbContext.SaveChangesAsync();

            HomeworkDto homeworkDto = new HomeworkDto
            {
                Id = updatedHomeworkEntity.Entity.Id,
                Title = updatedHomeworkEntity.Entity.Title,
                Instructions = updatedHomeworkEntity.Entity.Instructions,
                Points = updatedHomeworkEntity.Entity.Points,
                EndDate = updatedHomeworkEntity.Entity.EndDate,
                ClassId = updatedHomeworkEntity.Entity.ClassId,
            };

            return homeworkDto;
        }
    }
}
