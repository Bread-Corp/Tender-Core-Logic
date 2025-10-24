using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenderCoreLogic.Migrations
{
    /// <inheritdoc />
    public partial class sanralmodelupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "SanralTender");

            migrationBuilder.DropColumn(
                name: "Institution",
                table: "SanralTender");

            migrationBuilder.DropColumn(
                name: "TenderType",
                table: "SanralTender");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "SanralTender",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Institution",
                table: "SanralTender",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenderType",
                table: "SanralTender",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
