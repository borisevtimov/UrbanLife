using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanLife.Data.Data.Migrations
{
    public partial class AddedIdToPaymentsAndChangedKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Payments_PaymentNumber",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPayments_Payments_PaymentNumber",
                table: "UserPayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPayments",
                table: "UserPayments");

            migrationBuilder.DropIndex(
                name: "IX_UserPayments_PaymentNumber",
                table: "UserPayments");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PaymentNumber",
                table: "Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentNumber",
                table: "UserPayments");

            migrationBuilder.DropColumn(
                name: "PaymentNumber",
                table: "Purchases");

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "UserPayments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Purchases",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPayments",
                table: "UserPayments",
                columns: new[] { "UserId", "PaymentId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserPayments_PaymentId",
                table: "UserPayments",
                column: "PaymentId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserPayments_Payments_PaymentId",
                table: "UserPayments",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Payments_PaymentId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPayments_Payments_PaymentId",
                table: "UserPayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPayments",
                table: "UserPayments");

            migrationBuilder.DropIndex(
                name: "IX_UserPayments_PaymentId",
                table: "UserPayments");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PaymentId",
                table: "Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "UserPayments");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "PaymentNumber",
                table: "UserPayments",
                type: "char(19)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentNumber",
                table: "Purchases",
                type: "char(19)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPayments",
                table: "UserPayments",
                columns: new[] { "UserId", "PaymentNumber" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_UserPayments_PaymentNumber",
                table: "UserPayments",
                column: "PaymentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PaymentNumber",
                table: "Purchases",
                column: "PaymentNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Payments_PaymentNumber",
                table: "Purchases",
                column: "PaymentNumber",
                principalTable: "Payments",
                principalColumn: "Number");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPayments_Payments_PaymentNumber",
                table: "UserPayments",
                column: "PaymentNumber",
                principalTable: "Payments",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
