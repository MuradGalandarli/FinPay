using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinPay.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig17 : Migration
    {
        /// <inheritdoc />
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                //migrationBuilder.AddColumn<string>(
                //    name: "FromUserId",
                //    table: "PaypalTransactions",
                //    type: "text",
                //    nullable: false,
                //    defaultValue: "");

                //migrationBuilder.AddColumn<string>(
                //    name: "ToUserId",
                //    table: "PaypalTransactions",
                //    type: "text",
                //    nullable: false,
                //    defaultValue: "");
            }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Burada DropColumn qalmalıdır, çünki rollback lazım olduqda sütunlar silinəcək
            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "PaypalTransactions");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "PaypalTransactions");
        }
    }
}
