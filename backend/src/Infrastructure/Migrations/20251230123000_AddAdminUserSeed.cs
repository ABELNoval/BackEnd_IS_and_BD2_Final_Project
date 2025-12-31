using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddAdminUserSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert a default admin user (Id fixed GUID) with plain password stored in PasswordHash
            migrationBuilder.Sql(@"
                INSERT INTO `Users` (`Id`, `Name`, `Email`, `PasswordHash`, `RoleId`)
                VALUES ('11111111-1111-1111-1111-111111111111', 'Abel', 'abel@gmail.com', '1234yolo', 1);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM `Users` WHERE `Id` = '11111111-1111-1111-1111-111111111111';
            ");
        }
    }
}
