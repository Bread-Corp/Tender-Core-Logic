using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenderCoreLogic.Migrations
{
    /// <inheritdoc />
    public partial class dboptimisationadjusted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tender_Tag",
                table: "Tender_Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StandardUser_Tag",
                table: "StandardUser_Tag");

            migrationBuilder.AlterColumn<string>(
                name: "TagName",
                table: "Tags",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BaseTender",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "BaseTender",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "BaseTender",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tender_Tag",
                table: "Tender_Tag",
                columns: new[] { "TenderID", "TagID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StandardUser_Tag",
                table: "StandardUser_Tag",
                columns: new[] { "UserID", "TagID" });

            migrationBuilder.CreateIndex(
                name: "IX_User_Tender_FKUserID_FKTenderID",
                table: "User_Tender",
                columns: new[] { "FKUserID", "FKTenderID" });

            migrationBuilder.CreateIndex(
                name: "IX_Tender_Tag_TagID",
                table: "Tender_Tag",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TagName",
                table: "Tags",
                column: "TagName");

            migrationBuilder.CreateIndex(
                name: "IX_StandardUser_Tag_TagID",
                table: "StandardUser_Tag",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTender_ClosingDate",
                table: "BaseTender",
                column: "ClosingDate");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTender_PublishedDate",
                table: "BaseTender",
                column: "PublishedDate");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTender_Source",
                table: "BaseTender",
                column: "Source");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTender_Source_ClosingDate",
                table: "BaseTender",
                columns: new[] { "Source", "ClosingDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BaseTender_Status",
                table: "BaseTender",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTender_Status_ClosingDate",
                table: "BaseTender",
                columns: new[] { "Status", "ClosingDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BaseTender_Title",
                table: "BaseTender",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Tender_FKUserID_FKTenderID",
                table: "User_Tender");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tender_Tag",
                table: "Tender_Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tender_Tag_TagID",
                table: "Tender_Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tags_TagName",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StandardUser_Tag",
                table: "StandardUser_Tag");

            migrationBuilder.DropIndex(
                name: "IX_StandardUser_Tag_TagID",
                table: "StandardUser_Tag");

            migrationBuilder.DropIndex(
                name: "IX_BaseTender_ClosingDate",
                table: "BaseTender");

            migrationBuilder.DropIndex(
                name: "IX_BaseTender_PublishedDate",
                table: "BaseTender");

            migrationBuilder.DropIndex(
                name: "IX_BaseTender_Source",
                table: "BaseTender");

            migrationBuilder.DropIndex(
                name: "IX_BaseTender_Source_ClosingDate",
                table: "BaseTender");

            migrationBuilder.DropIndex(
                name: "IX_BaseTender_Status",
                table: "BaseTender");

            migrationBuilder.DropIndex(
                name: "IX_BaseTender_Status_ClosingDate",
                table: "BaseTender");

            migrationBuilder.DropIndex(
                name: "IX_BaseTender_Title",
                table: "BaseTender");

            migrationBuilder.AlterColumn<string>(
                name: "TagName",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BaseTender",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "BaseTender",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "BaseTender",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tender_Tag",
                table: "Tender_Tag",
                columns: new[] { "TagID", "TenderID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StandardUser_Tag",
                table: "StandardUser_Tag",
                columns: new[] { "TagID", "UserID" });
        }
    }
}
