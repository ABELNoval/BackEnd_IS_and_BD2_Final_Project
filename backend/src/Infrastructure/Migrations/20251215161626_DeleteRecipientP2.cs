using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteRecipientP2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_RecipientId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_RecipientId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Transfers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecipientId",
                table: "Transfers",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_RecipientId",
                table: "Transfers",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_RecipientId",
                table: "Transfers",
                column: "RecipientId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
