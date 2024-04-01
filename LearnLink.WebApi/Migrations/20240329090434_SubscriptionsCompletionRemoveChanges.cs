using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnLink.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class SubscriptionsCompletionRemoveChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CompletionProgress",
                table: "Subscriptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Subscriptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CompletionProgress",
                table: "Subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
