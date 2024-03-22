using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiService.Migrations
{
    public partial class RequestType1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestType",
                table: "ServiceRequests",
                newName: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ServiceRequests",
                newName: "RequestType");
        }
    }
}
