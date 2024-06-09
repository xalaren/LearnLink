using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnLink.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class SectionNullableForeignKeysChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_CodeContent_CodeContentId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_FileContent_FileContentId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_TextContent_TextContentId",
                table: "Sections");

            migrationBuilder.AlterColumn<int>(
                name: "TextContentId",
                table: "Sections",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "FileContentId",
                table: "Sections",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CodeContentId",
                table: "Sections",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_CodeContent_CodeContentId",
                table: "Sections",
                column: "CodeContentId",
                principalTable: "CodeContent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_FileContent_FileContentId",
                table: "Sections",
                column: "FileContentId",
                principalTable: "FileContent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_TextContent_TextContentId",
                table: "Sections",
                column: "TextContentId",
                principalTable: "TextContent",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_CodeContent_CodeContentId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_FileContent_FileContentId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_TextContent_TextContentId",
                table: "Sections");

            migrationBuilder.AlterColumn<int>(
                name: "TextContentId",
                table: "Sections",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FileContentId",
                table: "Sections",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CodeContentId",
                table: "Sections",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_CodeContent_CodeContentId",
                table: "Sections",
                column: "CodeContentId",
                principalTable: "CodeContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_FileContent_FileContentId",
                table: "Sections",
                column: "FileContentId",
                principalTable: "FileContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_TextContent_TextContentId",
                table: "Sections",
                column: "TextContentId",
                principalTable: "TextContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
