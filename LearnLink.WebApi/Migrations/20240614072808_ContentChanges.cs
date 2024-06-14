using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnLink.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ContentChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_FileContent_FileContentId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_TextContent_TextContentId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_FileContent_FileContentId",
                table: "Objectives");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_CodeContent_CodeContentId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_FileContent_FileContentId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_TextContent_TextContentId",
                table: "Sections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TextContent",
                table: "TextContent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileContent",
                table: "FileContent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeContent",
                table: "CodeContent");

            migrationBuilder.RenameTable(
                name: "TextContent",
                newName: "TextContents");

            migrationBuilder.RenameTable(
                name: "FileContent",
                newName: "FileContents");

            migrationBuilder.RenameTable(
                name: "CodeContent",
                newName: "CodeContents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextContents",
                table: "TextContents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileContents",
                table: "FileContents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeContents",
                table: "CodeContents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_FileContents_FileContentId",
                table: "Answers",
                column: "FileContentId",
                principalTable: "FileContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_TextContents_TextContentId",
                table: "Answers",
                column: "TextContentId",
                principalTable: "TextContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_FileContents_FileContentId",
                table: "Objectives",
                column: "FileContentId",
                principalTable: "FileContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_CodeContents_CodeContentId",
                table: "Sections",
                column: "CodeContentId",
                principalTable: "CodeContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_FileContents_FileContentId",
                table: "Sections",
                column: "FileContentId",
                principalTable: "FileContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_TextContents_TextContentId",
                table: "Sections",
                column: "TextContentId",
                principalTable: "TextContents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_FileContents_FileContentId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_TextContents_TextContentId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_FileContents_FileContentId",
                table: "Objectives");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_CodeContents_CodeContentId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_FileContents_FileContentId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_TextContents_TextContentId",
                table: "Sections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TextContents",
                table: "TextContents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileContents",
                table: "FileContents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeContents",
                table: "CodeContents");

            migrationBuilder.RenameTable(
                name: "TextContents",
                newName: "TextContent");

            migrationBuilder.RenameTable(
                name: "FileContents",
                newName: "FileContent");

            migrationBuilder.RenameTable(
                name: "CodeContents",
                newName: "CodeContent");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextContent",
                table: "TextContent",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileContent",
                table: "FileContent",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeContent",
                table: "CodeContent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_FileContent_FileContentId",
                table: "Answers",
                column: "FileContentId",
                principalTable: "FileContent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_TextContent_TextContentId",
                table: "Answers",
                column: "TextContentId",
                principalTable: "TextContent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_FileContent_FileContentId",
                table: "Objectives",
                column: "FileContentId",
                principalTable: "FileContent",
                principalColumn: "Id");

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
    }
}
