using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SphereWebsite.Migrations
{
    /// <inheritdoc />
    public partial class Tags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedTags",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedTags",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Posts");
        }
    }
}
