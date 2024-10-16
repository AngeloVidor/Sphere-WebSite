using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SphereWebsite.Migrations
{
    /// <inheritdoc />
    public partial class GroupPostsDownVoteOrUpVote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupPostsModelGroupPostID",
                table: "PostVoteModel",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Downvotes",
                table: "GroupPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Upvotes",
                table: "GroupPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteModel_GroupPostsModelGroupPostID",
                table: "PostVoteModel",
                column: "GroupPostsModelGroupPostID");

            migrationBuilder.AddForeignKey(
                name: "FK_PostVoteModel_GroupPosts_GroupPostsModelGroupPostID",
                table: "PostVoteModel",
                column: "GroupPostsModelGroupPostID",
                principalTable: "GroupPosts",
                principalColumn: "GroupPostID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostVoteModel_GroupPosts_GroupPostsModelGroupPostID",
                table: "PostVoteModel");

            migrationBuilder.DropIndex(
                name: "IX_PostVoteModel_GroupPostsModelGroupPostID",
                table: "PostVoteModel");

            migrationBuilder.DropColumn(
                name: "GroupPostsModelGroupPostID",
                table: "PostVoteModel");

            migrationBuilder.DropColumn(
                name: "Downvotes",
                table: "GroupPosts");

            migrationBuilder.DropColumn(
                name: "Upvotes",
                table: "GroupPosts");
        }
    }
}
