using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExistingCompletionDates : Migration
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

        }
    }
}
