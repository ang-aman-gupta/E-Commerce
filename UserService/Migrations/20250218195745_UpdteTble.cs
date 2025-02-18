using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class UpdteTble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_Users_UserId",
                table: "UserAddresses");

            migrationBuilder.DropIndex(
                name: "IX_UserAddresses_UserId",
                table: "UserAddresses");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserAddresses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserAddresses",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_UserId",
                table: "UserAddresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddresses_Users_UserId",
                table: "UserAddresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
