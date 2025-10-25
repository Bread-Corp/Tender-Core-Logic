using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenderCoreLogic.Migrations
{
    /// <inheritdoc />
    public partial class etendermodelupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "URL",
                table: "eTender",
                newName: "Province");

            migrationBuilder.RenameColumn(
                name: "Department",
                table: "eTender",
                newName: "OfficeLocation");

            migrationBuilder.AddColumn<bool>(
                name: "IsNotified",
                table: "User_Tender",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "eTender",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Audience",
                table: "eTender",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "eTender",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotified",
                table: "User_Tender");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "eTender");

            migrationBuilder.DropColumn(
                name: "Audience",
                table: "eTender");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "eTender");

            migrationBuilder.RenameColumn(
                name: "Province",
                table: "eTender",
                newName: "URL");

            migrationBuilder.RenameColumn(
                name: "OfficeLocation",
                table: "eTender",
                newName: "Department");
        }
    }
}
