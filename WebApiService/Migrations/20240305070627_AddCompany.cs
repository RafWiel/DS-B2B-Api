using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiService.Migrations
{
    public partial class AddCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyModelId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErpId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TaxNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Postal = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    City = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyModelId",
                table: "Customers",
                column: "CompanyModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Companies_CompanyModelId",
                table: "Customers",
                column: "CompanyModelId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Companies_CompanyModelId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CompanyModelId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CompanyModelId",
                table: "Customers");
        }
    }
}
