using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnLink.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ReviewChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerReviews_Users_ExpertUserId",
                table: "AnswerReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerReviews",
                table: "AnswerReviews");

            migrationBuilder.DropIndex(
                name: "IX_AnswerReviews_ExpertUserId",
                table: "AnswerReviews");

            migrationBuilder.DropColumn(
                name: "ExpertUserId",
                table: "AnswerReviews");

            migrationBuilder.AddColumn<int>(
                name: "ExpertUserId",
                table: "Reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerReviews",
                table: "AnswerReviews",
                columns: new[] { "AnswerId", "ReviewId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ExpertUserId",
                table: "Reviews",
                column: "ExpertUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_ExpertUserId",
                table: "Reviews",
                column: "ExpertUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_ExpertUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ExpertUserId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerReviews",
                table: "AnswerReviews");

            migrationBuilder.DropColumn(
                name: "ExpertUserId",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "ExpertUserId",
                table: "AnswerReviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerReviews",
                table: "AnswerReviews",
                columns: new[] { "AnswerId", "ReviewId", "ExpertUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerReviews_ExpertUserId",
                table: "AnswerReviews",
                column: "ExpertUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerReviews_Users_ExpertUserId",
                table: "AnswerReviews",
                column: "ExpertUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
