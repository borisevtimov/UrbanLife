using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanLife.Data.Data.Migrations
{
    public partial class FirstDatabaseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Number = table.Column<string>(type: "char(19)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "date", nullable: false),
                    CVC = table.Column<string>(type: "char(3)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Number);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentNumber = table.Column<string>(type: "char(19)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCard = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stops",
                columns: table => new
                {
                    Code = table.Column<string>(type: "char(4)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stops", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseLines",
                columns: table => new
                {
                    PurchaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LineId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseLines", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK_PurchaseLines_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseLines_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "UserPayments",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentNumber = table.Column<string>(type: "char(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPayments", x => new { x.UserId, x.PaymentNumber });
                    table.ForeignKey(
                        name: "FK_UserPayments_Payments_PaymentNumber",
                        column: x => x.PaymentNumber,
                        principalTable: "Payments",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPayments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseLines_LineId",
                table: "PurchaseLines",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_LineId",
                table: "TimeTables",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_StopCode",
                table: "TimeTables",
                column: "StopCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserPayments_PaymentNumber",
                table: "UserPayments",
                column: "PaymentNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseLines");

            migrationBuilder.DropTable(
                name: "TimeTables");

            migrationBuilder.DropTable(
                name: "UserPayments");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Lines");

            migrationBuilder.DropTable(
                name: "Stops");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
