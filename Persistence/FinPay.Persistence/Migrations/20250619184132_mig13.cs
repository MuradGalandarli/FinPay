using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinPay.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTransactions_AspNetUsers_ToUserId",
                table: "AppTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AppTransactions_ToUserId",
                table: "AppTransactions");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "AppTransactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ToUserId",
                table: "AppTransactions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppTransactions_ToUserId",
                table: "AppTransactions",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTransactions_AspNetUsers_ToUserId",
                table: "AppTransactions",
                column: "ToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
