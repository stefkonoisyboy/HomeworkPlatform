using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAttachmentEntityConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_HomeworkSubmissions_HomeworkSubmissionId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Homeworks_HomeworkId",
                table: "Attachments");

            migrationBuilder.AlterColumn<string>(
                name: "HomeworkSubmissionId",
                table: "Attachments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "HomeworkId",
                table: "Attachments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_HomeworkSubmissions_HomeworkSubmissionId",
                table: "Attachments",
                column: "HomeworkSubmissionId",
                principalTable: "HomeworkSubmissions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Homeworks_HomeworkId",
                table: "Attachments",
                column: "HomeworkId",
                principalTable: "Homeworks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_HomeworkSubmissions_HomeworkSubmissionId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Homeworks_HomeworkId",
                table: "Attachments");

            migrationBuilder.AlterColumn<string>(
                name: "HomeworkSubmissionId",
                table: "Attachments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HomeworkId",
                table: "Attachments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_HomeworkSubmissions_HomeworkSubmissionId",
                table: "Attachments",
                column: "HomeworkSubmissionId",
                principalTable: "HomeworkSubmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Homeworks_HomeworkId",
                table: "Attachments",
                column: "HomeworkId",
                principalTable: "Homeworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
