using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanLife.Data.Data.Migrations
{
    public partial class RemovedPaymentIdFromPurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Payments_PaymentId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PaymentId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Purchases");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Purchases",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PaymentId",
                table: "Purchases",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Payments_PaymentId",
                table: "Purchases",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id");
        }
    }
}
