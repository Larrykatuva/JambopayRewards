using Microsoft.EntityFrameworkCore.Migrations;

namespace JamboPayRewards.Migrations
{
    public partial class transaction_table_modification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionReference",
                table: "Transactions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionReference",
                table: "Transactions");
        }
    }
}
