using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenderCoreLogic.Migrations
{
    /// <inheritdoc />
    public partial class basetender_aisummaryfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AISummary",
                table: "BaseTender",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AISummary",
                table: "BaseTender");
        }
    }
}
