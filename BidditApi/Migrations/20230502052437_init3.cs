using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BidditApi.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arts_Users_UserId",
                table: "Arts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBids_Arts_ArtId",
                table: "UserBids");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBids_Bids_BidId",
                table: "UserBids");

            migrationBuilder.DropIndex(
                name: "IX_UserBids_ArtId",
                table: "UserBids");

            migrationBuilder.DropIndex(
                name: "IX_UserBids_BidId",
                table: "UserBids");

            migrationBuilder.DropIndex(
                name: "IX_Arts_UserId",
                table: "Arts");

            migrationBuilder.DropColumn(
                name: "ArtId",
                table: "UserBids");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArtId",
                table: "UserBids",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBids_ArtId",
                table: "UserBids",
                column: "ArtId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBids_BidId",
                table: "UserBids",
                column: "BidId");

            migrationBuilder.CreateIndex(
                name: "IX_Arts_UserId",
                table: "Arts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arts_Users_UserId",
                table: "Arts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBids_Arts_ArtId",
                table: "UserBids",
                column: "ArtId",
                principalTable: "Arts",
                principalColumn: "ArtId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBids_Bids_BidId",
                table: "UserBids",
                column: "BidId",
                principalTable: "Bids",
                principalColumn: "BidId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
