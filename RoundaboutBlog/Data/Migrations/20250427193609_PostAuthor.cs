using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoundaboutBlog.Data.Migrations
{
  /// <inheritdoc />
  public partial class PostAuthor : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
          name: "user_id",
          table: "posts",
          type: "text",
          nullable: false,
          defaultValue: "");

      migrationBuilder.CreateIndex(
          name: "ix_posts_user_id",
          table: "posts",
          column: "user_id");

      migrationBuilder.AddForeignKey(
          name: "fk_posts_users_user_id",
          table: "posts",
          column: "user_id",
          principalTable: "AspNetUsers",
          principalColumn: "id",
          onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "fk_posts_users_user_id",
          table: "posts");

      migrationBuilder.DropIndex(
          name: "ix_posts_user_id",
          table: "posts");

      migrationBuilder.DropColumn(
          name: "user_id",
          table: "posts");
    }
  }
}
