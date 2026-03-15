using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonalInfoOwnedEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "JobApplications",
                newName: "PersonalInfo_EmailAddress");

            migrationBuilder.AlterColumn<string>(
                name: "PersonalInfo_EmailAddress",
                table: "JobApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonalInfo_CountryCodeId",
                table: "JobApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_FullName",
                table: "JobApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_MobileNumber",
                table: "JobApplications",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_PersonalInfo_CountryCodeId",
                table: "JobApplications",
                column: "PersonalInfo_CountryCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Lookups_PersonalInfo_CountryCodeId",
                table: "JobApplications",
                column: "PersonalInfo_CountryCodeId",
                principalTable: "Lookups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Lookups_PersonalInfo_CountryCodeId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_PersonalInfo_CountryCodeId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_CountryCodeId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_FullName",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_MobileNumber",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "PersonalInfo_EmailAddress",
                table: "JobApplications",
                newName: "EmailAddress");

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
