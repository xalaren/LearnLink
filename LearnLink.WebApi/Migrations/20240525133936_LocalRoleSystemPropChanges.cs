using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnLink.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class LocalRoleSystemPropChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SystemRole",
                table: "Roles",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemRole",
                table: "Roles");
        }
    }
}
