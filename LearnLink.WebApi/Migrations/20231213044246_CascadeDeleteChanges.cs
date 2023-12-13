using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnLink.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModuleLessons_Modules_LessonId",
                table: "ModuleLessons");

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleLessons_Modules_ModuleId",
                table: "ModuleLessons",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModuleLessons_Modules_ModuleId",
                table: "ModuleLessons");

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleLessons_Modules_LessonId",
                table: "ModuleLessons",
                column: "LessonId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
