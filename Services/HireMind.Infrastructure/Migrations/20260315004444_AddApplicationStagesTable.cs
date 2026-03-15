using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationStagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentStageId",
                table: "JobApplications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinalStageId",
                table: "JobApplications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationStages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobApplicationId = table.Column<int>(type: "int", nullable: false),
                    HiringStageId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationStages_HiringStages_HiringStageId",
                        column: x => x.HiringStageId,
                        principalTable: "HiringStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationStages_JobApplications_JobApplicationId",
                        column: x => x.JobApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_CurrentStageId",
                table: "JobApplications",
                column: "CurrentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_FinalStageId",
                table: "JobApplications",
                column: "FinalStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStages_HiringStageId",
                table: "ApplicationStages",
                column: "HiringStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStages_JobApplicationId",
                table: "ApplicationStages",
                column: "JobApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_ApplicationStages_CurrentStageId",
                table: "JobApplications",
                column: "CurrentStageId",
                principalTable: "ApplicationStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_ApplicationStages_FinalStageId",
                table: "JobApplications",
                column: "FinalStageId",
                principalTable: "ApplicationStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_ApplicationStages_CurrentStageId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_ApplicationStages_FinalStageId",
                table: "JobApplications");

            migrationBuilder.DropTable(
                name: "ApplicationStages");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_CurrentStageId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_FinalStageId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "CurrentStageId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "FinalStageId",
                table: "JobApplications");
        }
    }
}
