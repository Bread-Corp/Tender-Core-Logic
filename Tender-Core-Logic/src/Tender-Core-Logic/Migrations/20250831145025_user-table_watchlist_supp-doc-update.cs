using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenderCoreLogic.Migrations
{
    /// <inheritdoc />
    public partial class usertable_watchlist_suppdocupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportingDocs",
                table: "BaseTender");

            migrationBuilder.AddColumn<Guid>(
                name: "StandardUserUserID",
                table: "Tag",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TenderUser",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSuperUser = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenderUser", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "StandardUser",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardUser", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_StandardUser_TenderUser_UserID",
                        column: x => x.UserID,
                        principalTable: "TenderUser",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Tender",
                columns: table => new
                {
                    WatchlistID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsWatched = table.Column<bool>(type: "bit", nullable: false),
                    UserTenderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FKTenderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FKUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Tender", x => x.WatchlistID);
                    table.ForeignKey(
                        name: "FK_User_Tender_BaseTender_FKTenderID",
                        column: x => x.FKTenderID,
                        principalTable: "BaseTender",
                        principalColumn: "TenderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Tender_TenderUser_FKUserID",
                        column: x => x.FKUserID,
                        principalTable: "TenderUser",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tag_StandardUserUserID",
                table: "Tag",
                column: "StandardUserUserID");

            migrationBuilder.CreateIndex(
                name: "IX_User_Tender_FKTenderID",
                table: "User_Tender",
                column: "FKTenderID");

            migrationBuilder.CreateIndex(
                name: "IX_User_Tender_FKTenderID_FKUserID",
                table: "User_Tender",
                columns: new[] { "FKTenderID", "FKUserID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Tender_FKUserID",
                table: "User_Tender",
                column: "FKUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_StandardUser_StandardUserUserID",
                table: "Tag",
                column: "StandardUserUserID",
                principalTable: "StandardUser",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_StandardUser_StandardUserUserID",
                table: "Tag");

            migrationBuilder.DropTable(
                name: "StandardUser");

            migrationBuilder.DropTable(
                name: "User_Tender");

            migrationBuilder.DropTable(
                name: "TenderUser");

            migrationBuilder.DropIndex(
                name: "IX_Tag_StandardUserUserID",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "StandardUserUserID",
                table: "Tag");

            migrationBuilder.AddColumn<string>(
                name: "SupportingDocs",
                table: "BaseTender",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
