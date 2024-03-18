using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiService.Migrations
{
    public partial class AddServiceRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Type",
                table: "Employees",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "Type",
                table: "Customers",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ServiceRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReminderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ordinal = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    PrefferedEmployeeId = table.Column<int>(type: "int", nullable: true),
                    PartnerCompanyId = table.Column<int>(type: "int", nullable: true),
                    Topic = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    SubmitType = table.Column<byte>(type: "tinyint", nullable: false),
                    Invoice = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_Companies_PartnerCompanyId",
                        column: x => x.PartnerCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceRequests_Employees_PrefferedEmployeeId",
                        column: x => x.PrefferedEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_CustomerId",
                table: "ServiceRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_EmployeeId",
                table: "ServiceRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_PartnerCompanyId",
                table: "ServiceRequests",
                column: "PartnerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_PrefferedEmployeeId",
                table: "ServiceRequests",
                column: "PrefferedEmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRequests");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Employees",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Customers",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }
    }
}
