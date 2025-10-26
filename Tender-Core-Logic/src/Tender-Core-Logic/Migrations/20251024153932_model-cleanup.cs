using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenderCoreLogic.Migrations
{
    /// <inheritdoc />
    public partial class modelcleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "eTender");

            migrationBuilder.RenameColumn(
                name: "FullNoticeText",
                table: "TransnetTender",
                newName: "TenderType");

            migrationBuilder.RenameColumn(
                name: "TenderType",
                table: "eTender",
                newName: "URL");

            migrationBuilder.AddColumn<string>(
                name: "Institution",
                table: "TransnetTender",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullTextNotice",
                table: "SanralTender",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Institution",
                table: "TransnetTender");

            migrationBuilder.DropColumn(
                name: "FullTextNotice",
                table: "SanralTender");

            migrationBuilder.RenameColumn(
                name: "TenderType",
                table: "TransnetTender",
                newName: "FullNoticeText");

            migrationBuilder.RenameColumn(
                name: "URL",
                table: "eTender",
                newName: "TenderType");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "eTender",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
