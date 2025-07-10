using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackTaro.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookletImagePathTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BookletImages",
                keyColumn: "Id",
                keyValue: -1,
                column: "ImagePath",
                value: "/uploads/booklets/birds_of_fire_booklet1.jpg");

            migrationBuilder.UpdateData(
                table: "Discs",
                keyColumn: "Id",
                keyValue: -1,
                column: "DiscImagePath",
                value: "/uploads/discs/birds_of_fire_disc1.png");

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "BackImagePath", "CoverImagePath" },
                values: new object[] { "/uploads/backs/birds_of_fire_back.jpg", "/uploads/covers/birds_of_fire.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BookletImages",
                keyColumn: "Id",
                keyValue: -1,
                column: "ImagePath",
                value: "/images/booklets/birds_of_fire_booklet1.jpg");

            migrationBuilder.UpdateData(
                table: "Discs",
                keyColumn: "Id",
                keyValue: -1,
                column: "DiscImagePath",
                value: "/images/discs/birds_of_fire_disc1.png");

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "BackImagePath", "CoverImagePath" },
                values: new object[] { "/images/covers/birds_of_fire_back.jpg", "/images/covers/birds_of_fire.jpg" });
        }
    }
}
