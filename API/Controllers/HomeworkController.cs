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

            HomeworkDto homeworkDto = await this.homeworkService
                .CreateAsync(createHomeworkDto, user.Id);

            return this.Ok(homeworkDto);
        }
    }
}
