using Core.Dtos.Attachment;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly ApplicationContext dbContext;

        public AttachmentService(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<AttachmentDto>> GetAllByHomeworkByIdAsync(string homeworkId)
        {
            ICollection<AttachmentDto> attachmentDtos = await dbContext.Attachments
                .Where(a => a.HomeworkId == homeworkId)
                .Select(a => new AttachmentDto
                {
                    Id = a.Id,
                    AttachmentUri = a.AttachmentUri,
                    CreatorId = a.CreatorId,
                    HomeworkId = a.HomeworkId,
                    HomeworkSubmissionId = a.HomeworkSubmissionId,
                })
                .ToListAsync();

            return attachmentDtos;
        }

        public async Task<ICollection<AttachmentDto>> GetAllByHomeworkSubmissionIdAsync(string homeworkSubmissionId)
        {
            ICollection<AttachmentDto> attachmentDtos = await dbContext.Attachments
                .Where(a => a.HomeworkSubmissionId == homeworkSubmissionId)
                .Select(a => new AttachmentDto
                {
                    Id = a.Id,
                    AttachmentUri = a.AttachmentUri,
                    CreatorId = a.CreatorId,
                    HomeworkId = a.HomeworkId,
                    HomeworkSubmissionId = a.HomeworkSubmissionId,
                })
                .ToListAsync();

            return attachmentDtos;
        }
    }
}
