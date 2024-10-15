using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SphereWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddLikesAndDislikesToPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Downvotes",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Upvotes",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PostVoteModel",
                columns: table => new
                {
                    VoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsUpvote = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostVoteModel", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_PostVoteModel_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostVoteModel_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteModel_PostId",
                table: "PostVoteModel",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteModel_UserId",
                table: "PostVoteModel",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostVoteModel");

            migrationBuilder.DropColumn(
                name: "Downvotes",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Upvotes",
                table: "Posts");
        }
    }
}
