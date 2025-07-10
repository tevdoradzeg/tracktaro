using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackTaro.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddBookletImagePathTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BookletImages",
                columns: new[] { "Id", "ImagePath", "ItemId" },
                values: new object[] { -1, "/images/booklets/birds_of_fire_booklet1.jpg", -1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookletImages",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
