using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiService.Migrations
{
    public partial class RequestType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ServiceRequests",
                newName: "RequestType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestType",
                table: "ServiceRequests",
                newName: "Type");
        }
    }
}
