using API.Attributes;
using API.Errors;
using API.Extensions;
using Core.Dtos.HomeworkSubmission;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class HomeworkSubmissionController : ApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHomeworkSubmissionService homeworkSubmissionService;
        private readonly IHomeworkService homeworkService;
        private readonly IClassService classService;

        public HomeworkSubmissionController(
            UserManager<ApplicationUser> userManager,
            IHomeworkSubmissionService homeworkSubmissionService,
            IHomeworkService homeworkService,
            IClassService classService)
        {
            this.userManager = userManager;
            this.homeworkSubmissionService = homeworkSubmissionService;
            this.homeworkService = homeworkService;
            this.classService = classService;
        }

        [CustomAuthorize(Constants.STUDENT_ROLE)]
        [HttpGet("by-class/{classId}")]
        public async Task<IActionResult> GetAllByClassForCurrentStudent([FromRoute] string classId)
        {
            if (!await this.classService
                .ExistsAsync(classId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.CLASS_NOT_FOUND));
            }

            ApplicationUser user = await this.userManager
              .FindByEmailFromClaimsPrincipalAsync(this.User);

            if (user == null)
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.USER_NOT_FOUND));
            }

            ICollection<HomeworkSubmissionDto> homeworkSubmissionDtos = await this.homeworkSubmissionService
                .GetAllByClassIdAndStudentIdAsync(classId, user.Id);

            return this.Ok(homeworkSubmissionDtos);
        }

        [CustomAuthorize(Constants.TEACHER_ROLE)]
        [HttpGet("by-homework/{homeworkId}")]
        public async Task<IActionResult> GetAllByHomework([FromRoute] string homeworkId)
        {
            if (!await this.homeworkService
                .ExistsAsync(homeworkId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.HOMEWORK_NOT_FOUND));
            }

            ICollection<HomeworkSubmissionDto> homeworkSubmissionDtos = await this.homeworkSubmissionService
                .GetAllByHomeworkIdAsync(homeworkId);

            return this.Ok(homeworkSubmissionDtos);
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE, Constants.STUDENT_ROLE)]
        [HttpGet("{homeworkSubmissionId}")]
        public async Task<IActionResult> GetById([FromRoute] string homeworkSubmissionId)
        {
            if (!await this.homeworkSubmissionService
                .ExistsAsync(homeworkSubmissionId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.HOMEWORK_SUBMISSION_NOT_FOUND));
            }

            HomeworkSubmissionDto homeworkSubmissionDto = await this.homeworkSubmissionService
                .GetByIdAsync(homeworkSubmissionId);

            return this.Ok(homeworkSubmissionDto);
        }

        [CustomAuthorize(Constants.STUDENT_ROLE)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateHomeworkSubmissionDto createHomeworkSubmissionDto)
        {
            if (!await this.homeworkService
               .ExistsAsync(createHomeworkSubmissionDto.HomeworkId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.HOMEWORK_NOT_FOUND));
            }

            ApplicationUser user = await this.userManager
              .FindByEmailFromClaimsPrincipalAsync(this.User);

            if (user == null)
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.USER_NOT_FOUND));
            }

            HomeworkSubmissionBasicDto homeworkSubmissionBasicDto = await this.homeworkSubmissionService
                .CreateAsync(createHomeworkSubmissionDto, user.Id);

            return this.Ok(homeworkSubmissionBasicDto);
        }
    }
}
