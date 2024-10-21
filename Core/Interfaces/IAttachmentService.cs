using Core.Dtos.Attachment;

namespace Core.Interfaces
{
    public interface IAttachmentService
    {
        Task<ICollection<AttachmentDto>> GetAllByHomeworkByIdAsync(string homeworkId);

        Task<ICollection<AttachmentDto>> GetAllByHomeworkSubmissionIdAsync(string homeworkSubmissionId);
    }
}
