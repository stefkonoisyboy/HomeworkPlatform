using API.Attributes;
using API.Errors;
using Core.Dtos.Class;
using Core.Dtos.User;
using Core.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ClassController : ApiController
    {
        private readonly IUserService userService;
        private readonly IClassService classService;

        public ClassController(
            IUserService userService,
            IClassService classService)
        {
            this.userService = userService;
            this.classService = classService;
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE, Constants.STUDENT_ROLE)]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            ICollection<ClassDto> classDtos = await this.classService
                .GetAllAsync();

            return this.Ok(classDtos);
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE, Constants.STUDENT_ROLE)]
        [HttpGet("{classId}")]
        public async Task<IActionResult> GetById([FromRoute] string classId)
        {
            if (!await this.classService
                .ExistsAsync(classId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.CLASS_NOT_FOUND));
            }

            ClassDto classDto = await this.classService
                .GetByIdAsync(classId);

            return this.Ok(classDto);
        }

        [CustomAuthorize(Constants.TEACHER_ROLE)]
        [HttpGet("{classId}/students")]
        public async Task<IActionResult> GetAllStudentsByClass([FromRoute] string classId)
        {
            if (!await this.classService
                .ExistsAsync(classId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.CLASS_NOT_FOUND));
            }

            ICollection<UserDto> userDtos = await this.classService
                .GetAllStudentsByClassIdAsync(classId);

            return this.Ok(userDtos);
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE)]
        [HttpGet("{classId}/teachers")]
        public async Task<IActionResult> GetAllTeachersByClass([FromRoute] string classId)
        {
            if (!await this.classService
                .ExistsAsync(classId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.CLASS_NOT_FOUND));
            }

            ICollection<UserDto> userDtos = await this.classService
                .GetAllTeachersByClassIdAsync(classId);

            return this.Ok(userDtos);
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE)]
        [HttpPost("{classId}/assign-student/{studentId}")]
        public async Task<IActionResult> AssignStudentToClass([FromRoute] string classId, [FromRoute] string studentId)
        {
            if (!await this.classService
               .ExistsAsync(classId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.CLASS_NOT_FOUND));
            }

            if (!await this.userService
               .ExistsAsync(studentId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.USER_NOT_FOUND));
            }

            if (await this.classService
                .CheckIfStudentIsAssignedForClass(classId, studentId))
            {
                return this.Conflict(new ApiResponse(StatusCodes.Status409Conflict, Constants.STUDENT_ALREADY_ASSIGNED));
            }

            await this.classService
                .AssignStudentAsync(classId, studentId);

            return this.NoContent();
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE)]
        [HttpPost("{classId}/assign-teacher/{teacherId}")]
        public async Task<IActionResult> AssignTeacherToClass([FromRoute] string classId, [FromRoute] string teacherId)
        {
            if (!await this.classService
               .ExistsAsync(classId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.CLASS_NOT_FOUND));
            }

            if (!await this.userService
               .ExistsAsync(teacherId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.USER_NOT_FOUND));
            }

            if (await this.classService
               .CheckIfTeacherIsAssignedForClass(classId, teacherId))
            {
                return this.Conflict(new ApiResponse(StatusCodes.Status409Conflict, Constants.TEACHER_ALREADY_ASSIGNED));
            }

            await this.classService
                .AssignTeacherAsync(classId, teacherId);

            return this.NoContent();
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE)]
        [HttpPost("{classId}/remove-student/{studentId}")]
        public async Task<IActionResult> RemoveStudentFromClass([FromRoute] string classId, [FromRoute] string studentId)
        {
            if (!await this.classService
               .ExistsAsync(classId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.CLASS_NOT_FOUND));
            }

            if (!await this.userService
               .ExistsAsync(studentId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.USER_NOT_FOUND));
            }

            if (!await this.classService
                .CheckIfStudentIsAssignedForClass(classId, studentId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.NON_EXISTENT_STUDENT_IN_CLASS));
            }

            await this.classService
                .RemoveStudentAsync(classId, studentId);

            return this.NoContent();
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE)]
        [HttpPost("{classId}/remove-teacher/{teacherId}")]
        public async Task<IActionResult> RemoveTeacherFromClass([FromRoute] string classId, [FromRoute] string teacherId)
        {
            if (!await this.classService
               .ExistsAsync(classId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.CLASS_NOT_FOUND));
            }

            if (!await this.userService
               .ExistsAsync(teacherId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.USER_NOT_FOUND));
            }

            if (!await this.classService
              .CheckIfTeacherIsAssignedForClass(classId, teacherId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.NON_EXISTENT_TEACHER_IN_CLASS));
            }

            if (await this.classService
              .CheckIfTeacherIsCreatorForClass(classId, teacherId))
            {
                return this.BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, Constants.CANNOT_REMOVE_CREATOR_TEACHER));
            }

            await this.classService
                .RemoveTeacherAsync(classId, teacherId);

            return this.NoContent();
        }
    }
}
