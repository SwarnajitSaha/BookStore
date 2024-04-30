using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Bug2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailTable_OrderHeadersTable_OrderHeaderId",
                table: "OrderDetailTable");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetailTable_OrderHeaderId",
                table: "OrderDetailTable");

            migrationBuilder.DropColumn(
                name: "OrderHeaderId",
                table: "OrderDetailTable");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetailTable_OrderHaderId",
                table: "OrderDetailTable",
                column: "OrderHaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailTable_OrderHeadersTable_OrderHaderId",
                table: "OrderDetailTable",
                column: "OrderHaderId",
                principalTable: "OrderHeadersTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailTable_OrderHeadersTable_OrderHaderId",
                table: "OrderDetailTable");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetailTable_OrderHaderId",
                table: "OrderDetailTable");

            migrationBuilder.AddColumn<int>(
                name: "OrderHeaderId",
                table: "OrderDetailTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetailTable_OrderHeaderId",
                table: "OrderDetailTable",
                column: "OrderHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailTable_OrderHeadersTable_OrderHeaderId",
                table: "OrderDetailTable",
                column: "OrderHeaderId",
                principalTable: "OrderHeadersTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
