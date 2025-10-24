using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenderCoreLogic.Migrations
{
    /// <inheritdoc />
    public partial class suppdocs_tags_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_StandardUser_StandardUserUserID",
                table: "Tag");

            migrationBuilder.DropTable(
                name: "BaseTenderTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_StandardUserUserID",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "StandardUserUserID",
                table: "Tag");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "TagID");

            migrationBuilder.CreateTable(
                name: "StandardUser_Tag",
                columns: table => new
                {
                    TagID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardUser_Tag", x => new { x.TagID, x.UserID });
                    table.ForeignKey(
                        name: "FK_StandardUser_Tag_StandardUser_UserID",
                        column: x => x.UserID,
                        principalTable: "StandardUser",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandardUser_Tag_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportingDocs",
                columns: table => new
                {
                    SupportingDocID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportingDocs", x => x.SupportingDocID);
                    table.ForeignKey(
                        name: "FK_SupportingDocs_BaseTender_TenderID",
                        column: x => x.TenderID,
                        principalTable: "BaseTender",
                        principalColumn: "TenderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tender_Tag",
                columns: table => new
                {
                    TagID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tender_Tag", x => new { x.TagID, x.TenderID });
                    table.ForeignKey(
                        name: "FK_Tender_Tag_BaseTender_TenderID",
                        column: x => x.TenderID,
                        principalTable: "BaseTender",
                        principalColumn: "TenderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tender_Tag_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StandardUser_Tag_UserID",
                table: "StandardUser_Tag",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SupportingDocs_TenderID",
                table: "SupportingDocs",
                column: "TenderID");

            migrationBuilder.CreateIndex(
                name: "IX_Tender_Tag_TenderID",
                table: "Tender_Tag",
                column: "TenderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StandardUser_Tag");

            migrationBuilder.DropTable(
                name: "SupportingDocs");

            migrationBuilder.DropTable(
                name: "Tender_Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.AddColumn<Guid>(
                name: "StandardUserUserID",
                table: "Tag",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "TagID");

            migrationBuilder.CreateTable(
                name: "BaseTenderTag",
                columns: table => new
                {
                    TagsTagID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TendersTenderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseTenderTag", x => new { x.TagsTagID, x.TendersTenderID });
                    table.ForeignKey(
                        name: "FK_BaseTenderTag_BaseTender_TendersTenderID",
                        column: x => x.TendersTenderID,
                        principalTable: "BaseTender",
                        principalColumn: "TenderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseTenderTag_Tag_TagsTagID",
                        column: x => x.TagsTagID,
                        principalTable: "Tag",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tag_StandardUserUserID",
                table: "Tag",
                column: "StandardUserUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTenderTag_TendersTenderID",
                table: "BaseTenderTag",
                column: "TendersTenderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_StandardUser_StandardUserUserID",
                table: "Tag",
                column: "StandardUserUserID",
                principalTable: "StandardUser",
                principalColumn: "UserID");
        }
    }
}
