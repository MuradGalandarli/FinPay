using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinPay.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromUserId",
                table: "PaypalTransactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ToUserId",
                table: "PaypalTransactions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "PaypalTransactions");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "PaypalTransactions");
        }
    }
}
