using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenderCoreLogic.Migrations
{
    /// <inheritdoc />
    public partial class newmodels_etenderupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "eTender");

            migrationBuilder.DropColumn(
                name: "ProcurementMethod",
                table: "eTender");

            migrationBuilder.DropColumn(
                name: "ProcurementMethodDetails",
                table: "eTender");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "eTender");

            migrationBuilder.RenameColumn(
                name: "Tenderer",
                table: "eTender",
                newName: "TenderType");

            migrationBuilder.RenameColumn(
                name: "ProcuringEntity",
                table: "eTender",
                newName: "Department");

            migrationBuilder.CreateTable(
                name: "SanralTender",
                columns: table => new
                {
                    TenderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Institution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenderType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanralTender", x => x.TenderID);
                    table.ForeignKey(
                        name: "FK_SanralTender_BaseTender_TenderID",
                        column: x => x.TenderID,
                        principalTable: "BaseTender",
                        principalColumn: "TenderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SarsTender",
                columns: table => new
                {
                    TenderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BriefingSession = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SarsTender", x => x.TenderID);
                    table.ForeignKey(
                        name: "FK_SarsTender_BaseTender_TenderID",
                        column: x => x.TenderID,
                        principalTable: "BaseTender",
                        principalColumn: "TenderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransnetTender",
                columns: table => new
                {
                    TenderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNoticeText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransnetTender", x => x.TenderID);
                    table.ForeignKey(
                        name: "FK_TransnetTender_BaseTender_TenderID",
                        column: x => x.TenderID,
                        principalTable: "BaseTender",
                        principalColumn: "TenderID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SanralTender");

            migrationBuilder.DropTable(
                name: "SarsTender");

            migrationBuilder.DropTable(
                name: "TransnetTender");

            migrationBuilder.RenameColumn(
                name: "TenderType",
                table: "eTender",
                newName: "Tenderer");

            migrationBuilder.RenameColumn(
                name: "Department",
                table: "eTender",
                newName: "ProcuringEntity");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "eTender",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcurementMethod",
                table: "eTender",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcurementMethodDetails",
                table: "eTender",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "eTender",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
