using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCompletionDatesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update existing records: set CompletionDate = DecommissionDate where NULL
            migrationBuilder.Sql(@"
                UPDATE EquipmentDecommissions 
                SET CompletionDate = DecommissionDate 
                WHERE CompletionDate IS NULL
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert: set CompletionDate back to NULL
            migrationBuilder.Sql(@"
                UPDATE EquipmentDecommissions 
                SET CompletionDate = NULL
            ");
        }
    }
}
