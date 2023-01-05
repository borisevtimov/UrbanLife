using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanLife.Data.Data.Migrations
{
    public partial class AddedReceiptPathAndNextStop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeTables");

            migrationBuilder.AddColumn<string>(
                name: "ReceiptPath",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LineId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StopCode = table.Column<string>(type: "char(4)", nullable: false),
                    NextStopCode = table.Column<string>(type: "char(4)", nullable: true),
                    Arrival = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsWeekday = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_Stops_NextStopCode",
                        column: x => x.NextStopCode,
                        principalTable: "Stops",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Schedules_Stops_StopCode",
                        column: x => x.StopCode,
                        principalTable: "Stops",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_LineId",
                table: "Schedules",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_NextStopCode",
                table: "Schedules",
                column: "NextStopCode");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_StopCode",
                table: "Schedules",
                column: "StopCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropColumn(
                name: "ReceiptPath",
                table: "Purchases");

            migrationBuilder.CreateTable(
                name: "TimeTables",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LineId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StopCode = table.Column<string>(type: "char(4)", nullable: false),
                    Arrival = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsWeekday = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeTables_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeTables_Stops_StopCode",
                        column: x => x.StopCode,
                        principalTable: "Stops",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_LineId",
                table: "TimeTables",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_StopCode",
                table: "TimeTables",
                column: "StopCode");
        }
    }
}
