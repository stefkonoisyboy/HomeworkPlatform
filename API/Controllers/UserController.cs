using API.Attributes;
using API.Errors;
using API.Extensions;
using Core.Dtos.User;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController : ApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public UserController(
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            this.userManager = userManager;
            this.userService = userService;
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE, Constants.STUDENT_ROLE)]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById([FromRoute] string userId)
        {
            if (!await this.userService
                .ExistsAsync(userId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.USER_NOT_FOUND));
            }

            ApplicationUser user = await this.userManager
                .FindByEmailFromClaimsPrincipalAsync(this.User);

            if (user == null)
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.USER_NOT_FOUND));
            }

            if (user.Id != userId)
            {
                return this.BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, Constants.CURRENT_USER_NOT_MATCHING));
            }

            UserDto userDto = await this.userService
                .GetByIdAsync(userId);

            return this.Ok(userDto);
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE, Constants.STUDENT_ROLE)]
        [HttpPut("{userId}")]
        public async Task<IActionResult> Edit([FromRoute] string userId, [FromBody] EditUserDto editUserDto)
        {
            if (!await this.userService
               .ExistsAsync(editUserDto.Id))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.USER_NOT_FOUND));
            }

            ApplicationUser user = await this.userManager
                .FindByEmailFromClaimsPrincipalAsync(this.User);

            if (user == null)
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.USER_NOT_FOUND));
            }

            if (user.Id != userId)
            {
                return this.BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, Constants.CURRENT_USER_NOT_MATCHING));
            }

            UserDto userDto = await this.userService
                .UpdateAsync(editUserDto);

            return this.Ok(userDto);
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE)]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete([FromRoute] string userId)
        {
            if (!await this.userService
              .ExistsAsync(userId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.USER_NOT_FOUND));
            }

            await this.userService
                .DeleteAsync(userId);

            return this.NoContent();
        }
    }
}
