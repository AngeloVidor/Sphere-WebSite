using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SphereWebsite.Migrations
{
    /// <inheritdoc />
    public partial class GroupCommentsRN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupFeedComments",
                columns: table => new
                {
                    CommentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    GroupPostID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupFeedComments", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_GroupFeedComments_GroupPosts_GroupPostID",
                        column: x => x.GroupPostID,
                        principalTable: "GroupPosts",
                        principalColumn: "GroupPostID",
                        onDelete: ReferentialAction.NoAction); 
                    table.ForeignKey(
                        name: "FK_GroupFeedComments_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupFeedComments_GroupPostID",
                table: "GroupFeedComments",
                column: "GroupPostID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupFeedComments_UserID",
                table: "GroupFeedComments",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupFeedComments");
        }
    }
}
