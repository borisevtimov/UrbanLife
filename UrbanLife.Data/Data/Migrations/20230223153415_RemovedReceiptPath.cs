using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanLife.Data.Data.Migrations
{
    public partial class RemovedReceiptPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptPath",
                table: "Purchases");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiptPath",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
