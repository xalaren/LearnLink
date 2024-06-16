using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnLink.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AnswerUploadDateChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Answers");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "Answers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "Answers");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Answers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");
        }
    }
}
