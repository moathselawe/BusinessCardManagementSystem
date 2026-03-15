using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPropsInHireStages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailTemplate",
                table: "HiringStages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExamQuestionsJson",
                table: "HiringStages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InterviewQuestionsJson",
                table: "HiringStages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ViaId",
                table: "HiringStages",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailTemplate",
                table: "HiringStages");

            migrationBuilder.DropColumn(
                name: "ExamQuestionsJson",
                table: "HiringStages");

            migrationBuilder.DropColumn(
                name: "InterviewQuestionsJson",
                table: "HiringStages");

            migrationBuilder.DropColumn(
                name: "ViaId",
                table: "HiringStages");
        }
    }
}
