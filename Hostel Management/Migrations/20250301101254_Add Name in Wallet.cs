using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hostel_Management.Migrations
{
    /// <inheritdoc />
    public partial class AddNameinWallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Wallets");
        }
    }
}
