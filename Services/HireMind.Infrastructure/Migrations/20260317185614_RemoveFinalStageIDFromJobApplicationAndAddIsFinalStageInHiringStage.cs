using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFinalStageIDFromJobApplicationAndAddIsFinalStageInHiringStage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_ApplicationStages_FinalStageId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_FinalStageId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "FinalStageId",
                table: "JobApplications");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalStage",
                table: "HiringStages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinalStage",
                table: "HiringStages");

            migrationBuilder.AddColumn<int>(
                name: "FinalStageId",
                table: "JobApplications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_FinalStageId",
                table: "JobApplications",
                column: "FinalStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_ApplicationStages_FinalStageId",
                table: "JobApplications",
                column: "FinalStageId",
                principalTable: "ApplicationStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
