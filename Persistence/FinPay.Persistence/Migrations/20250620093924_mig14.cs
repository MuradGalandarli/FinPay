using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinPay.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTransactions_AspNetUsers_FromUserId",
                table: "AppTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PaypalTransactions_AspNetUsers_UserId",
                table: "PaypalTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PaypalTransactions_UserId",
                table: "PaypalTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AppTransactions_FromUserId",
                table: "AppTransactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PaypalTransactions");

            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "AppTransactions");

            migrationBuilder.AddColumn<int>(
                name: "UserAccountId",
                table: "PaypalTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PaypalEmail",
                table: "AppTransactions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "UserAccountId",
                table: "AppTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaypalTransactions_UserAccountId",
                table: "PaypalTransactions",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AppTransactions_UserAccountId",
                table: "AppTransactions",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTransactions_UserAccount_UserAccountId",
                table: "AppTransactions",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaypalTransactions_UserAccount_UserAccountId",
                table: "PaypalTransactions",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTransactions_UserAccount_UserAccountId",
                table: "AppTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PaypalTransactions_UserAccount_UserAccountId",
                table: "PaypalTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PaypalTransactions_UserAccountId",
                table: "PaypalTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AppTransactions_UserAccountId",
                table: "AppTransactions");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "PaypalTransactions");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "AppTransactions");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PaypalTransactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PaypalEmail",
                table: "AppTransactions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromUserId",
                table: "AppTransactions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaypalTransactions_UserId",
                table: "PaypalTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppTransactions_FromUserId",
                table: "AppTransactions",
                column: "FromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTransactions_AspNetUsers_FromUserId",
                table: "AppTransactions",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaypalTransactions_AspNetUsers_UserId",
                table: "PaypalTransactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
