using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SphereWebsite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVotesRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "GroupPostsVoteModel",
                columns: table => new
                {
                    VoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsUpvote = table.Column<bool>(type: "bit", nullable: false),
                    GroupPostsVoteModelVoteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPostsVoteModel", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_GroupPostsVoteModel_GroupPostsVoteModel_GroupPostsVoteModelVoteId",
                        column: x => x.GroupPostsVoteModelVoteId,
                        principalTable: "GroupPostsVoteModel",
                        principalColumn: "VoteId");
                    table.ForeignKey(
                        name: "FK_GroupPostsVoteModel_GroupPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "GroupPosts",
                        principalColumn: "GroupPostID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPostsVoteModel_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupPostsVoteModel_GroupPostsVoteModelVoteId",
                table: "GroupPostsVoteModel",
                column: "GroupPostsVoteModelVoteId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPostsVoteModel_PostId",
                table: "GroupPostsVoteModel",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPostsVoteModel_UserId",
                table: "GroupPostsVoteModel",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupPostsVoteModel");

            migrationBuilder.AddColumn<int>(
                name: "GroupPostsModelGroupPostID",
                table: "PostVoteModel",
                type: "int",
                nullable: true);

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
    }
}
