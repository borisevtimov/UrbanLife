using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanLife.Data.Data.Migrations
{
    public partial class AddedMissedForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Purchases",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PaymentNumber",
                table: "Purchases",
                column: "PaymentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_UserId",
                table: "Purchases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_AspNetUsers_UserId",
                table: "Purchases",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Payments_PaymentNumber",
                table: "Purchases",
                column: "PaymentNumber",
                principalTable: "Payments",
                principalColumn: "Number");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_AspNetUsers_UserId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Payments_PaymentNumber",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PaymentNumber",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_UserId",
                table: "Purchases");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
