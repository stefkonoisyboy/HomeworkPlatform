using API.Attributes;
using API.Errors;
using API.Extensions;
using Core.Dtos.Homework;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class HomeworkController : ApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IClassService classService;
        private readonly IHomeworkService homeworkService;

        public HomeworkController(
            UserManager<ApplicationUser> userManager,
            IClassService classService,
            IHomeworkService homeworkService)
        {
            this.userManager = userManager;
            this.classService = classService;
            this.homeworkService = homeworkService;
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE, Constants.STUDENT_ROLE)]
        [HttpGet("by-class/{classId}")]
        public async Task<IActionResult> GetByClassId([FromRoute] string classId)
        {
            if (!await this.classService
                .ExistsAsync(classId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.CLASS_NOT_FOUND));
            }

            ICollection<HomeworkDto> homeworkDtos = await this.homeworkService
                .GetAllByClassIdAsync(classId);

            return this.Ok(homeworkDtos);
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE, Constants.STUDENT_ROLE)]
        [HttpGet("{homeworkId}")]
        public async Task<IActionResult> GetById([FromRoute] string homeworkId)
        {
            if (!await this.homeworkService
                .ExistsAsync(homeworkId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.HOMEWORK_NOT_FOUND));
            }

            HomeworkDto homeworkDto = await this.homeworkService
                .GetByIdAsync(homeworkId);

            return this.Ok(homeworkDto);
        }

        [CustomAuthorize(Constants.TEACHER_ROLE)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateHomeworkDto createHomeworkDto)
        {
            if (!await this.classService
                .ExistsAsync(createHomeworkDto.ClassId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.CLASS_NOT_FOUND));
            }

            ApplicationUser user = await this.userManager
                .FindByEmailFromClaimsPrincipalAsync(this.User);

            if (user == null)
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.USER_NOT_FOUND));
            }

            HomeworkDto homeworkDto = await this.homeworkService
                .CreateAsync(createHomeworkDto, user.Id);

            return this.Ok(homeworkDto);
        }

        [CustomAuthorize(Constants.TEACHER_ROLE)]
        [HttpPut("{homeworkId}")]
        public async Task<IActionResult> Edit([FromBody] EditHomeworkDto editHomeworkDto, [FromRoute] string homeworkId)
        {
            if (!await this.homeworkService
                .ExistsAsync(editHomeworkDto.Id))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.HOMEWORK_NOT_FOUND));
            }

            HomeworkDto homeworkDto = await this.homeworkService
                .UpdateAsync(editHomeworkDto);

            return this.Ok(homeworkDto);
        }

        [CustomAuthorize(Constants.TEACHER_ROLE)]
        [HttpDelete("{homeworkId}")]
        public async Task<IActionResult> Delete([FromRoute] string homeworkId)
        {
            if (!await this.homeworkService
               .ExistsAsync(homeworkId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.HOMEWORK_NOT_FOUND));
            }

            await this.homeworkService
                .DeleteAsync(homeworkId);

            return this.NoContent();
        }
    }
}
