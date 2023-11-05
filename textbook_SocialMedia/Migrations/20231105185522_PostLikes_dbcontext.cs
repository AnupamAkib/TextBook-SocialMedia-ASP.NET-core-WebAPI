using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace textbook_SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class PostLikes_dbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostLike_Posts_PostID",
                table: "PostLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostLike",
                table: "PostLike");

            migrationBuilder.RenameTable(
                name: "PostLike",
                newName: "PostLikes");

            migrationBuilder.RenameIndex(
                name: "IX_PostLike_PostID",
                table: "PostLikes",
                newName: "IX_PostLikes_PostID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostLikes",
                table: "PostLikes",
                column: "PostLikeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLikes_Posts_PostID",
                table: "PostLikes",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "PostID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostLikes_Posts_PostID",
                table: "PostLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostLikes",
                table: "PostLikes");

            migrationBuilder.RenameTable(
                name: "PostLikes",
                newName: "PostLike");

            migrationBuilder.RenameIndex(
                name: "IX_PostLikes_PostID",
                table: "PostLike",
                newName: "IX_PostLike_PostID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostLike",
                table: "PostLike",
                column: "PostLikeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLike_Posts_PostID",
                table: "PostLike",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "PostID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
