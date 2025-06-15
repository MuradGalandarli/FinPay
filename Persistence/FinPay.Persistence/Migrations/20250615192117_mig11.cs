using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinPay.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardBalances_AspNetUsers_UserId",
                table: "CardBalances");

            migrationBuilder.DropIndex(
                name: "IX_CardBalances_UserId",
                table: "CardBalances");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CardBalances");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CardBalances",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CardBalances");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CardBalances",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CardBalances_UserId",
                table: "CardBalances",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CardBalances_AspNetUsers_UserId",
                table: "CardBalances",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
