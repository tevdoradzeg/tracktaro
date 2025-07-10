using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackTaro.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscImagePathTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Discs",
                keyColumn: "Id",
                keyValue: -1,
                column: "DiscImagePath",
                value: "/images/discs/birds_of_fire_disc1.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Discs",
                keyColumn: "Id",
                keyValue: -1,
                column: "DiscImagePath",
                value: "/fakepath.png");
        }
    }
}
