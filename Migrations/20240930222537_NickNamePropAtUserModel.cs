using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SphereWebsite.Migrations
{
    /// <inheritdoc />
    public partial class NickNamePropAtUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserModelID",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserModelID",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UserModelID",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserModelID",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserModelID",
                table: "Posts",
                column: "UserModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserModelID",
                table: "Posts",
                column: "UserModelID",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
