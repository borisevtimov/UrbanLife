using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanLife.Data.Data.Migrations
{
    public partial class FixedPrimaryKeyOnPurchaseLineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseLines",
                table: "PurchaseLines");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseLines",
                table: "PurchaseLines",
                columns: new[] { "PurchaseId", "LineId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseLines",
                table: "PurchaseLines");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseLines",
                table: "PurchaseLines",
                column: "PurchaseId");
        }
    }
}
