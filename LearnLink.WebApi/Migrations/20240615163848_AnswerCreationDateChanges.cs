using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnLink.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AnswerCreationDateChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Answers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Answers");
        }
    }
}
