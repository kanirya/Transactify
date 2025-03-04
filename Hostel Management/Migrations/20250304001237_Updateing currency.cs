using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hostel_Management.Migrations
{
    /// <inheritdoc />
    public partial class Updateingcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "Currencies");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Currencies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Currencies",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Currencies");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Currencies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "Currencies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Currencies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
