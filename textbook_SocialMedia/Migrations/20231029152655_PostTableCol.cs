using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace textbook_SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class PostTableCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeDate",
                table: "Posts",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeDate",
                table: "Posts");
        }
    }
}
