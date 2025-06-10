using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RoundaboutBlog.Data.Migrations
{
  /// <inheritdoc />
  public partial class AddComment : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "fk_posts_users_user_id",
          table: "posts");

      migrationBuilder.AlterColumn<string>(
          name: "user_id",
          table: "posts",
          type: "text",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "text");

      migrationBuilder.AlterColumn<string>(
          name: "title",
          table: "posts",
          type: "character varying(200)",
          maxLength: 200,
          nullable: false,
          oldClrType: typeof(string),
          oldType: "text");

      migrationBuilder.AlterColumn<string>(
          name: "content",
          table: "posts",
          type: "character varying(5000)",
          maxLength: 5000,
          nullable: false,
          oldClrType: typeof(string),
          oldType: "text");

      migrationBuilder.CreateTable(
          name: "comments",
          columns: table => new
          {
            comment_id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
            title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
            content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
            created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
            user_id = table.Column<string>(type: "text", nullable: true),
            post_id = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_comments", x => x.comment_id);
            table.ForeignKey(
                      name: "fk_comments_posts_post_id",
                      column: x => x.post_id,
                      principalTable: "posts",
                      principalColumn: "post_id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "fk_comments_users_user_id",
                      column: x => x.user_id,
                      principalTable: "AspNetUsers",
                      principalColumn: "id");
          });

      migrationBuilder.CreateIndex(
          name: "ix_comments_post_id",
          table: "comments",
          column: "post_id");

      migrationBuilder.CreateIndex(
          name: "ix_comments_user_id",
          table: "comments",
          column: "user_id");

      migrationBuilder.AddForeignKey(
          name: "fk_posts_users_user_id",
          table: "posts",
          column: "user_id",
          principalTable: "AspNetUsers",
          principalColumn: "id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "fk_posts_users_user_id",
          table: "posts");

      migrationBuilder.DropTable(
          name: "comments");

      migrationBuilder.AlterColumn<string>(
          name: "user_id",
          table: "posts",
          type: "text",
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "text",
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "title",
          table: "posts",
          type: "text",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "character varying(200)",
          oldMaxLength: 200);

      migrationBuilder.AlterColumn<string>(
          name: "content",
          table: "posts",
          type: "text",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "character varying(5000)",
          oldMaxLength: 5000);

      migrationBuilder.AddForeignKey(
          name: "fk_posts_users_user_id",
          table: "posts",
          column: "user_id",
          principalTable: "AspNetUsers",
          principalColumn: "id",
          onDelete: ReferentialAction.Cascade);
    }
  }
}
