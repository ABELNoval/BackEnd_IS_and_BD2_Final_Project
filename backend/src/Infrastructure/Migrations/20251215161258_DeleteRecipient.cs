using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteRecipient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentDecommissions_RecipientId",
                table: "EquipmentDecommissions",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentDecommissions_Employees_RecipientId",
                table: "EquipmentDecommissions",
                column: "RecipientId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_RecipientId",
                table: "Transfers",
                column: "RecipientId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentDecommissions_Employees_RecipientId",
                table: "EquipmentDecommissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_RecipientId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_RecipientId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentDecommissions_RecipientId",
                table: "EquipmentDecommissions");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Transfers");
        }
    }
}
