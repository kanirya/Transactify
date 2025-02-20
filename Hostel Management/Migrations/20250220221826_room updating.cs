using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hostel_Management.Migrations
{
    /// <inheritdoc />
    public partial class roomupdating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_Students_RequestedBy",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Rooms_RoomId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceRequests_RequestedBy",
                table: "MaintenanceRequests");

            migrationBuilder.AddColumn<int>(
                name: "RequestedStudentId",
                table: "MaintenanceRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_RequestedStudentId",
                table: "MaintenanceRequests",
                column: "RequestedStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_Students_RequestedStudentId",
                table: "MaintenanceRequests",
                column: "RequestedStudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Rooms_RoomId",
                table: "Students",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_Students_RequestedStudentId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Rooms_RoomId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceRequests_RequestedStudentId",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "RequestedStudentId",
                table: "MaintenanceRequests");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_RequestedBy",
                table: "MaintenanceRequests",
                column: "RequestedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_Students_RequestedBy",
                table: "MaintenanceRequests",
                column: "RequestedBy",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Rooms_RoomId",
                table: "Students",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
