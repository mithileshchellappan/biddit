using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BidditApi.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtHash",
                table: "Arts");

            migrationBuilder.AddColumn<string>(
                name: "ArtURL",
                table: "Arts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtURL",
                table: "Arts");

            migrationBuilder.AddColumn<byte[]>(
                name: "ArtHash",
                table: "Arts",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
