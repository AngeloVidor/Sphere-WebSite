using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SphereWebsite.Migrations
{
    /// <inheritdoc />
    public partial class vs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupPostsModelGroupPostID",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupPosts",
                columns: table => new
                {
                    GroupPostID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectedTags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPosts", x => x.GroupPostID);
                    table.ForeignKey(
                        name: "FK_GroupPosts_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_GroupPostsModelGroupPostID",
                table: "Comments",
                column: "GroupPostsModelGroupPostID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPosts_GroupId",
                table: "GroupPosts",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPosts_UserId",
                table: "GroupPosts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_GroupPosts_GroupPostsModelGroupPostID",
                table: "Comments",
                column: "GroupPostsModelGroupPostID",
                principalTable: "GroupPosts",
                principalColumn: "GroupPostID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_GroupPosts_GroupPostsModelGroupPostID",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "GroupPosts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_GroupPostsModelGroupPostID",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "GroupPostsModelGroupPostID",
                table: "Comments");
        }
    }
}
