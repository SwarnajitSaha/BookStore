using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Bug1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CompanyTable_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PamentStatus",
                table: "OrderHeadersTable",
                newName: "PaymentStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CompanyTable_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "CompanyTable",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CompanyTable_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "OrderHeadersTable",
                newName: "PamentStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CompanyTable_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "CompanyTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
