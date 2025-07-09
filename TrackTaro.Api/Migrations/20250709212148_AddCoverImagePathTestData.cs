using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackTaro.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverImagePathTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "BackImagePath", "CoverImagePath" },
                values: new object[] { "/images/covers/birds_of_fire_back.jpg", "/images/covers/birds_of_fire.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "BackImagePath", "CoverImagePath" },
                values: new object[] { "", "" });
        }
    }
}
