using API.Attributes;
using API.Errors;
using Core.Dtos.Attachment;
using Core.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AttachmentController : ApiController
    {
        private readonly IAttachmentService attachmentService;
        private readonly IHomeworkService homeworkService;
        private readonly IHomeworkSubmissionService homeworkSubmissionService;

        public AttachmentController(
            IAttachmentService attachmentService,
            IHomeworkService homeworkService,
            IHomeworkSubmissionService homeworkSubmissionService)
        {
            this.attachmentService = attachmentService;
            this.homeworkService = homeworkService;
            this.homeworkSubmissionService = homeworkSubmissionService;
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE, Constants.STUDENT_ROLE)]
        [HttpGet("by-homework/{homeworkId}")]
        public async Task<IActionResult> GetAllByHomeworkId([FromRoute] string homeworkId)
        {
            if (!await this.homeworkService
              .ExistsAsync(homeworkId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.HOMEWORK_NOT_FOUND));
            }

            ICollection<AttachmentDto> attachmentDtos = await this.attachmentService
                .GetAllByHomeworkByIdAsync(homeworkId);

            return this.Ok(attachmentDtos);
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE, Constants.STUDENT_ROLE)]
        [HttpGet("by-homeworksubmission/{homeworkSubmissionId}")]
        public async Task<IActionResult> GetAllByHomeworkSubmissionId([FromRoute] string homeworkSubmissionId)
        {
            if (!await this.homeworkSubmissionService
                .ExistsAsync(homeworkSubmissionId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.HOMEWORK_SUBMISSION_NOT_FOUND));
            }

            ICollection<AttachmentDto> attachmentDtos = await this.attachmentService
                .GetAllByHomeworkSubmissionIdAsync(homeworkSubmissionId);

            return this.Ok(attachmentDtos);
        }
    }
}
