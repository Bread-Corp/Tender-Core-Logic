using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenderCoreLogic.Migrations
{
    /// <inheritdoc />
    public partial class transnetmodelupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "TransnetTender",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "TransnetTender");
        }
    }
}
