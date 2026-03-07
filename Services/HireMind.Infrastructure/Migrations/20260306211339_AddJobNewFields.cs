using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJobNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractTypeId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IndustrySectorId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationTypeId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkPlaceId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractTypeId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "IndustrySectorId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "OrganizationTypeId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "WorkPlaceId",
                table: "Jobs");
        }
    }
}
