using API.Attributes;
using API.Errors;
using Core.Dtos.Grade;
using Core.Entities;
using Core.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class GradeController : ApiController
    {
        private readonly IHomeworkSubmissionService homeworkSubmissionService;
        private readonly IGradeService gradeService;

        public GradeController(
            IHomeworkSubmissionService homeworkSubmissionService,
            IGradeService gradeService)
        {
            this.homeworkSubmissionService = homeworkSubmissionService;
            this.gradeService = gradeService;
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE, Constants.STUDENT_ROLE)]
        [HttpGet("{homeworkSubmissionId}")]
        public async Task<IActionResult> GetByHomeworkSubmission([FromRoute] string homeworkSubmissionId)
        {
            if (!await this.homeworkSubmissionService
                .ExistsAsync(homeworkSubmissionId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.HOMEWORK_SUBMISSION_NOT_FOUND));
            }

            GradeDto gradeDto = await this.gradeService
                .GetByHomeworkSubmissionIdAsync(homeworkSubmissionId);

            return this.Ok(gradeDto);
        }

        [CustomAuthorize(Constants.TEACHER_ROLE)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGradeDto createGradeDto)
        {
            if (!await this.homeworkSubmissionService
               .ExistsAsync(createGradeDto.HomeworkSubmissionId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.HOMEWORK_SUBMISSION_NOT_FOUND));
            }

            if (createGradeDto.Points.HasValue)
            {
                if (double.IsNaN(createGradeDto.Points.Value))
                {
                    return this.BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, Constants.POINTS_SHOULD_BE_NUMERIC));
                }

                if (await this.gradeService
                    .CheckExistingGradeAsync(createGradeDto.HomeworkSubmissionId))
                {
                    return this.Conflict(new ApiResponse(StatusCodes.Status409Conflict, Constants.GRADE_ALREADY_CREATED));
                }

                if (!await this.gradeService
                    .CheckIfHomeworkHasPointsAsync(createGradeDto.HomeworkSubmissionId))
                {
                    return this.BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, Constants.CANNOT_GRADE_HOMEWORK_SUBMISSION_WITHOUT_POINTS));
                }

                if (!await this.gradeService
                   .CheckIfGradeIsValidAsync(createGradeDto.Points.Value, createGradeDto.HomeworkSubmissionId))
                {
                    return this.BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, Constants.GRADE_POINTS_EXCEEDED));
                }
            }

            GradeDto gradeDto = await this.gradeService
                .CreateAsync(createGradeDto);

            return this.Ok(gradeDto);
        }

        [CustomAuthorize(Constants.ADMINISTRATOR_ROLE, Constants.TEACHER_ROLE)]
        [HttpPut("{gradeId}")]
        public async Task<IActionResult> Edit([FromRoute] string gradeId, [FromBody] EditGradeDto editGradeDto)
        {
            if (!await this.homeworkSubmissionService
               .ExistsAsync(editGradeDto.HomeworkSubmissionId))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.HOMEWORK_SUBMISSION_NOT_FOUND));
            }

            if (!await this.gradeService
                .ExistsAsync(editGradeDto.Id))
            {
                return this.NotFound(new ApiResponse(StatusCodes.Status404NotFound, Constants.GRADE_NOT_FOUND));
            }

            if (editGradeDto.Points.HasValue)
            {
                if (double.IsNaN(editGradeDto.Points.Value))
                {
                    return this.BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, Constants.POINTS_SHOULD_BE_NUMERIC));
                }

                if (!await this.gradeService
                    .CheckIfHomeworkHasPointsAsync(editGradeDto.HomeworkSubmissionId))
                {
                    return this.BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, Constants.CANNOT_GRADE_HOMEWORK_SUBMISSION_WITHOUT_POINTS));
                }

                if (!await this.gradeService
                   .CheckIfGradeIsValidAsync(editGradeDto.Points.Value, editGradeDto.HomeworkSubmissionId))
                {
                    return this.BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, Constants.GRADE_POINTS_EXCEEDED));
                }
            }

            GradeDto gradeDto = await this.gradeService
                .UpdateAsync(editGradeDto);

            return this.Ok(gradeDto);
        }
    }
}
