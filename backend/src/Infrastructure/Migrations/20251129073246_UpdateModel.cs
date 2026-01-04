using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assessments_Directors_directorid",
                table: "assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_assessments_Technicals_technicalid",
                table: "assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_departments_Responsibles_ResponsibleId",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "FK_departments_sections_SectionId",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Directors_Users_id",
                table: "Directors");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_departments_departmentid",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_equipments_departments_departmentid",
                table: "equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_equipments_equipmenttypes_equipmenttypeid",
                table: "equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_maintenances_Technicals_technicalid",
                table: "maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_maintenances_equipments_equipmentid",
                table: "maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Responsibles_Employees_id",
                table: "Responsibles");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicals_Users_id",
                table: "Technicals");

            migrationBuilder.DropForeignKey(
                name: "FK_transfers_departments_destinyid",
                table: "transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_transfers_departments_originid",
                table: "transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_transfers_equipments_equipmentid",
                table: "transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_roles_roleid",
                table: "Users");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "technicaldowntimes");

            migrationBuilder.DropTable(
                name: "destinytypes");

            migrationBuilder.DropIndex(
                name: "IX_Users_roleid",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transfers",
                table: "transfers");

            migrationBuilder.DropIndex(
                name: "IX_transfers_destinyid",
                table: "transfers");

            migrationBuilder.DropIndex(
                name: "IX_transfers_originid",
                table: "transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sections",
                table: "sections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_maintenances",
                table: "maintenances");

            migrationBuilder.DropIndex(
                name: "IX_maintenances_technicalid",
                table: "maintenances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_equipmenttypes",
                table: "equipmenttypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_equipments",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "IX_equipments_departmentid",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "IX_equipments_equipmenttypeid",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "IX_Employees_departmentid",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_departments",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "IX_departments_ResponsibleId",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "IX_departments_SectionId",
                table: "departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_assessments",
                table: "assessments");

            migrationBuilder.DropIndex(
                name: "IX_assessments_directorid",
                table: "assessments");

            migrationBuilder.DropColumn(
                name: "destinyid",
                table: "transfers");

            migrationBuilder.DropColumn(
                name: "originid",
                table: "transfers");

            migrationBuilder.DropColumn(
                name: "type",
                table: "maintenances");

            migrationBuilder.DropColumn(
                name: "departmentid",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "coment",
                table: "assessments");

            migrationBuilder.DropColumn(
                name: "punctuation",
                table: "assessments");

            migrationBuilder.RenameTable(
                name: "transfers",
                newName: "Transfers");

            migrationBuilder.RenameTable(
                name: "sections",
                newName: "Sections");

            migrationBuilder.RenameTable(
                name: "maintenances",
                newName: "Maintenances");

            migrationBuilder.RenameTable(
                name: "equipmenttypes",
                newName: "EquipmentTypes");

            migrationBuilder.RenameTable(
                name: "equipments",
                newName: "Equipments");

            migrationBuilder.RenameTable(
                name: "departments",
                newName: "Departments");

            migrationBuilder.RenameTable(
                name: "assessments",
                newName: "Assessments");

            migrationBuilder.RenameColumn(
                name: "roleid",
                table: "Users",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "gmail",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_Users_gmail",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameColumn(
                name: "equipmentid",
                table: "Transfers",
                newName: "EquipmentId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Transfers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "datetime",
                table: "Transfers",
                newName: "TransferDate");

            migrationBuilder.RenameIndex(
                name: "IX_transfers_equipmentid",
                table: "Transfers",
                newName: "IX_Transfers_EquipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_transfers_datetime",
                table: "Transfers",
                newName: "IX_Transfers_TransferDate");

            migrationBuilder.RenameColumn(
                name: "specialty",
                table: "Technicals",
                newName: "Specialty");

            migrationBuilder.RenameColumn(
                name: "experience",
                table: "Technicals",
                newName: "Experience");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Technicals",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Sections",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Sections",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Responsibles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "technicalid",
                table: "Maintenances",
                newName: "TechnicalId");

            migrationBuilder.RenameColumn(
                name: "equipmentid",
                table: "Maintenances",
                newName: "EquipmentId");

            migrationBuilder.RenameColumn(
                name: "cost",
                table: "Maintenances",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Maintenances",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "datetime",
                table: "Maintenances",
                newName: "MaintenanceDate");

            migrationBuilder.RenameIndex(
                name: "IX_maintenances_equipmentid",
                table: "Maintenances",
                newName: "IX_Maintenances_EquipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_maintenances_datetime",
                table: "Maintenances",
                newName: "IX_Maintenances_MaintenanceDate");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "EquipmentTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "EquipmentTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Equipments",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "equipmenttypeid",
                table: "Equipments",
                newName: "EquipmentTypeId");

            migrationBuilder.RenameColumn(
                name: "departmentid",
                table: "Equipments",
                newName: "DepartmentId");

            migrationBuilder.RenameColumn(
                name: "acquisitiondate",
                table: "Equipments",
                newName: "AcquisitionDate");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Equipments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "Equipments",
                newName: "StateId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Employees",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Directors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Departments",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Departments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "technicalid",
                table: "Assessments",
                newName: "TechnicalId");

            migrationBuilder.RenameColumn(
                name: "directorid",
                table: "Assessments",
                newName: "DirectorId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Assessments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Assessments",
                newName: "AssessmentDate");

            migrationBuilder.RenameIndex(
                name: "IX_assessments_technicalid",
                table: "Assessments",
                newName: "IX_Assessments_TechnicalId");

            migrationBuilder.RenameIndex(
                name: "IX_assessments_date",
                table: "Assessments",
                newName: "IX_Assessments_AssessmentDate");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "EquipmentId",
                table: "Transfers",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Transfers",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Transfers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ResponsibleId",
                table: "Transfers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "SourceDepartmentId",
                table: "Transfers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TargetDepartmentId",
                table: "Transfers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Technicals",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sections",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Sections",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Responsibles",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "TechnicalId",
                table: "Maintenances",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "EquipmentId",
                table: "Maintenances",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Maintenances",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Maintenances",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "MaintenanceTypeId",
                table: "Maintenances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EquipmentTypes",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "EquipmentTypes",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Equipments",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "EquipmentTypeId",
                table: "Equipments",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "DepartmentId",
                table: "Equipments",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Equipments",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "StateId",
                table: "Equipments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "LocationTypeId",
                table: "Equipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Employees",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Directors",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "SectionId",
                table: "Departments",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "ResponsibleId",
                table: "Departments",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Departments",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TechnicalId",
                table: "Assessments",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "DirectorId",
                table: "Assessments",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Assessments",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Assessments",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "Score",
                table: "Assessments",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transfers",
                table: "Transfers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sections",
                table: "Sections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maintenances",
                table: "Maintenances",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentTypes",
                table: "EquipmentTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipments",
                table: "Equipments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assessments",
                table: "Assessments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EquipmentDecommissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EquipmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TechnicalId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DepartmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DestinyTypeId = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DecommissionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Reason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentDecommissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentDecommissions_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTypes_Name",
                table: "EquipmentTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentDecommissions_DecommissionDate",
                table: "EquipmentDecommissions",
                column: "DecommissionDate");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentDecommissions_EquipmentId",
                table: "EquipmentDecommissions",
                column: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Technicals_TechnicalId",
                table: "Assessments",
                column: "TechnicalId",
                principalTable: "Technicals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Directors_Users_Id",
                table: "Directors",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_Id",
                table: "Employees",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Equipments_EquipmentId",
                table: "Maintenances",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Responsibles_Employees_Id",
                table: "Responsibles",
                column: "Id",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Technicals_Users_Id",
                table: "Technicals",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Equipments_EquipmentId",
                table: "Transfers",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Technicals_TechnicalId",
                table: "Assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_Directors_Users_Id",
                table: "Directors");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_Id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Equipments_EquipmentId",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Responsibles_Employees_Id",
                table: "Responsibles");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicals_Users_Id",
                table: "Technicals");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Equipments_EquipmentId",
                table: "Transfers");

            migrationBuilder.DropTable(
                name: "EquipmentDecommissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transfers",
                table: "Transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sections",
                table: "Sections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Maintenances",
                table: "Maintenances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentTypes",
                table: "EquipmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentTypes_Name",
                table: "EquipmentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipments",
                table: "Equipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assessments",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "ResponsibleId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "SourceDepartmentId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "TargetDepartmentId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "MaintenanceTypeId",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "LocationTypeId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Assessments");

            migrationBuilder.RenameTable(
                name: "Transfers",
                newName: "transfers");

            migrationBuilder.RenameTable(
                name: "Sections",
                newName: "sections");

            migrationBuilder.RenameTable(
                name: "Maintenances",
                newName: "maintenances");

            migrationBuilder.RenameTable(
                name: "EquipmentTypes",
                newName: "equipmenttypes");

            migrationBuilder.RenameTable(
                name: "Equipments",
                newName: "equipments");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "departments");

            migrationBuilder.RenameTable(
                name: "Assessments",
                newName: "assessments");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Users",
                newName: "roleid");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "gmail");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "Users",
                newName: "IX_Users_gmail");

            migrationBuilder.RenameColumn(
                name: "EquipmentId",
                table: "transfers",
                newName: "equipmentid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "transfers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TransferDate",
                table: "transfers",
                newName: "datetime");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_EquipmentId",
                table: "transfers",
                newName: "IX_transfers_equipmentid");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_TransferDate",
                table: "transfers",
                newName: "IX_transfers_datetime");

            migrationBuilder.RenameColumn(
                name: "Specialty",
                table: "Technicals",
                newName: "specialty");

            migrationBuilder.RenameColumn(
                name: "Experience",
                table: "Technicals",
                newName: "experience");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Technicals",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "sections",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sections",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Responsibles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TechnicalId",
                table: "maintenances",
                newName: "technicalid");

            migrationBuilder.RenameColumn(
                name: "EquipmentId",
                table: "maintenances",
                newName: "equipmentid");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "maintenances",
                newName: "cost");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "maintenances",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "MaintenanceDate",
                table: "maintenances",
                newName: "datetime");

            migrationBuilder.RenameIndex(
                name: "IX_Maintenances_EquipmentId",
                table: "maintenances",
                newName: "IX_maintenances_equipmentid");

            migrationBuilder.RenameIndex(
                name: "IX_Maintenances_MaintenanceDate",
                table: "maintenances",
                newName: "IX_maintenances_datetime");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "equipmenttypes",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "equipmenttypes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "equipments",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "EquipmentTypeId",
                table: "equipments",
                newName: "equipmenttypeid");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "equipments",
                newName: "departmentid");

            migrationBuilder.RenameColumn(
                name: "AcquisitionDate",
                table: "equipments",
                newName: "acquisitiondate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "equipments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "equipments",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Employees",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Directors",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "departments",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "departments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TechnicalId",
                table: "assessments",
                newName: "technicalid");

            migrationBuilder.RenameColumn(
                name: "DirectorId",
                table: "assessments",
                newName: "directorid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "assessments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "AssessmentDate",
                table: "assessments",
                newName: "date");

            migrationBuilder.RenameIndex(
                name: "IX_Assessments_TechnicalId",
                table: "assessments",
                newName: "IX_assessments_technicalid");

            migrationBuilder.RenameIndex(
                name: "IX_Assessments_AssessmentDate",
                table: "assessments",
                newName: "IX_assessments_date");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "equipmentid",
                table: "transfers",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "transfers",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "destinyid",
                table: "transfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "originid",
                table: "transfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Technicals",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "sections",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "sections",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Responsibles",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "technicalid",
                table: "maintenances",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "equipmentid",
                table: "maintenances",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<float>(
                name: "cost",
                table: "maintenances",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "maintenances",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "maintenances",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "equipmenttypes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "equipmenttypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "equipments",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "equipmenttypeid",
                table: "equipments",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "departmentid",
                table: "equipments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "equipments",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "state",
                table: "equipments",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Employees",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "departmentid",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Directors",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "departments",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "ResponsibleId",
                table: "departments",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "departments",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "technicalid",
                table: "assessments",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "directorid",
                table: "assessments",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "assessments",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "coment",
                table: "assessments",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<float>(
                name: "punctuation",
                table: "assessments",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddPrimaryKey(
                name: "PK_transfers",
                table: "transfers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sections",
                table: "sections",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_maintenances",
                table: "maintenances",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_equipmenttypes",
                table: "equipmenttypes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_equipments",
                table: "equipments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_departments",
                table: "departments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_assessments",
                table: "assessments",
                column: "id");

            migrationBuilder.CreateTable(
                name: "destinytypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_destinytypes", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "technicaldowntimes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    departmentid = table.Column<int>(type: "int", nullable: false),
                    destinytypeid = table.Column<int>(type: "int", nullable: false),
                    equipmentid = table.Column<int>(type: "int", nullable: false),
                    technicalid = table.Column<int>(type: "int", nullable: false),
                    cause = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_technicaldowntimes", x => x.id);
                    table.ForeignKey(
                        name: "FK_technicaldowntimes_Technicals_technicalid",
                        column: x => x.technicalid,
                        principalTable: "Technicals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_technicaldowntimes_departments_departmentid",
                        column: x => x.departmentid,
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_technicaldowntimes_destinytypes_destinytypeid",
                        column: x => x.destinytypeid,
                        principalTable: "destinytypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_technicaldowntimes_equipments_equipmentid",
                        column: x => x.equipmentid,
                        principalTable: "equipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_roleid",
                table: "Users",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "IX_transfers_destinyid",
                table: "transfers",
                column: "destinyid");

            migrationBuilder.CreateIndex(
                name: "IX_transfers_originid",
                table: "transfers",
                column: "originid");

            migrationBuilder.CreateIndex(
                name: "IX_maintenances_technicalid",
                table: "maintenances",
                column: "technicalid");

            migrationBuilder.CreateIndex(
                name: "IX_equipments_departmentid",
                table: "equipments",
                column: "departmentid");

            migrationBuilder.CreateIndex(
                name: "IX_equipments_equipmenttypeid",
                table: "equipments",
                column: "equipmenttypeid");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_departmentid",
                table: "Employees",
                column: "departmentid");

            migrationBuilder.CreateIndex(
                name: "IX_departments_ResponsibleId",
                table: "departments",
                column: "ResponsibleId");

            migrationBuilder.CreateIndex(
                name: "IX_departments_SectionId",
                table: "departments",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_assessments_directorid",
                table: "assessments",
                column: "directorid");

            migrationBuilder.CreateIndex(
                name: "IX_technicaldowntimes_date",
                table: "technicaldowntimes",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "IX_technicaldowntimes_departmentid",
                table: "technicaldowntimes",
                column: "departmentid");

            migrationBuilder.CreateIndex(
                name: "IX_technicaldowntimes_destinytypeid",
                table: "technicaldowntimes",
                column: "destinytypeid");

            migrationBuilder.CreateIndex(
                name: "IX_technicaldowntimes_equipmentid",
                table: "technicaldowntimes",
                column: "equipmentid");

            migrationBuilder.CreateIndex(
                name: "IX_technicaldowntimes_technicalid",
                table: "technicaldowntimes",
                column: "technicalid");

            migrationBuilder.AddForeignKey(
                name: "FK_assessments_Directors_directorid",
                table: "assessments",
                column: "directorid",
                principalTable: "Directors",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_assessments_Technicals_technicalid",
                table: "assessments",
                column: "technicalid",
                principalTable: "Technicals",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_departments_Responsibles_ResponsibleId",
                table: "departments",
                column: "ResponsibleId",
                principalTable: "Responsibles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_departments_sections_SectionId",
                table: "departments",
                column: "SectionId",
                principalTable: "sections",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Directors_Users_id",
                table: "Directors",
                column: "id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_id",
                table: "Employees",
                column: "id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_departments_departmentid",
                table: "Employees",
                column: "departmentid",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_equipments_departments_departmentid",
                table: "equipments",
                column: "departmentid",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_equipments_equipmenttypes_equipmenttypeid",
                table: "equipments",
                column: "equipmenttypeid",
                principalTable: "equipmenttypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_maintenances_Technicals_technicalid",
                table: "maintenances",
                column: "technicalid",
                principalTable: "Technicals",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_maintenances_equipments_equipmentid",
                table: "maintenances",
                column: "equipmentid",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Responsibles_Employees_id",
                table: "Responsibles",
                column: "id",
                principalTable: "Employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Technicals_Users_id",
                table: "Technicals",
                column: "id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transfers_departments_destinyid",
                table: "transfers",
                column: "destinyid",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transfers_departments_originid",
                table: "transfers",
                column: "originid",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transfers_equipments_equipmentid",
                table: "transfers",
                column: "equipmentid",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_roles_roleid",
                table: "Users",
                column: "roleid",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
