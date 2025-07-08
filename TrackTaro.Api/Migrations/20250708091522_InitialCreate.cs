using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrackTaro.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Publisher = table.Column<string>(type: "TEXT", nullable: false),
                    Label = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    CoverImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    BackImagePath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ArtistId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookletImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookletImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookletImages_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    DiscImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discs_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemArtists",
                columns: table => new
                {
                    ArtistsId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemArtists", x => new { x.ArtistsId, x.ItemsId });
                    table.ForeignKey(
                        name: "FK_ItemArtists_Artists_ArtistsId",
                        column: x => x.ArtistsId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemArtists_Items_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscArtists",
                columns: table => new
                {
                    ArtistsId = table.Column<int>(type: "INTEGER", nullable: false),
                    DiscsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscArtists", x => new { x.ArtistsId, x.DiscsId });
                    table.ForeignKey(
                        name: "FK_DiscArtists_Artists_ArtistsId",
                        column: x => x.ArtistsId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscArtists_Discs_DiscsId",
                        column: x => x.DiscsId,
                        principalTable: "Discs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    DiscNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    TrackNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DiscId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tracks_Discs_DiscId",
                        column: x => x.DiscId,
                        principalTable: "Discs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackArtists",
                columns: table => new
                {
                    ArtistsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TracksId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackArtists", x => new { x.ArtistsId, x.TracksId });
                    table.ForeignKey(
                        name: "FK_TrackArtists_Artists_ArtistsId",
                        column: x => x.ArtistsId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackArtists_Tracks_TracksId",
                        column: x => x.TracksId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "Country", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { -1, "United States", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mahavishnu Orchestra", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "BackImagePath", "CoverImagePath", "CreatedAt", "Description", "Label", "Name", "Publisher", "Type", "UpdatedAt", "Year" },
                values: new object[] { -1, "", "", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "As with the group's previous album, The Inner Mounting Flame, Birds of Fire consists solely of compositions by John McLaughlin. These include the track \"Miles Beyond (Miles Davis)\", which McLaughlin dedicated to his friend and former bandleader.", "", "Birds of Fire", "", 0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1973 });

            migrationBuilder.InsertData(
                table: "Discs",
                columns: new[] { "Id", "CreatedAt", "DiscImagePath", "ItemId", "Number", "Type", "UpdatedAt" },
                values: new object[] { -1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "/fakepath.png", -1, 1, 0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "ItemArtists",
                columns: new[] { "ArtistsId", "ItemsId" },
                values: new object[] { -1, -1 });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "CreatedAt", "DiscId", "DiscNumber", "Duration", "Name", "TrackNumber", "UpdatedAt" },
                values: new object[,]
                {
                    { -10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -1, 1, new TimeSpan(0, 0, 2, 10, 0), "Resolution", 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { -9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -1, 1, new TimeSpan(0, 0, 3, 54, 0), "Open Country Joy", 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { -8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -1, 1, new TimeSpan(0, 0, 5, 2, 0), "Sanctuary", 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { -7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -1, 1, new TimeSpan(0, 0, 9, 55, 0), "One Word", 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { -6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -1, 1, new TimeSpan(0, 0, 1, 57, 0), "Hope", 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { -5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -1, 1, new TimeSpan(0, 0, 3, 20, 0), "Thousand Islad Park", 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { -4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -1, 1, new TimeSpan(0, 0, 0, 22, 0), "Sapphire Bullets of Pure Love", 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { -3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -1, 1, new TimeSpan(0, 0, 2, 54, 0), "Celestial Terrestrial Commuters", 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { -2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -1, 1, new TimeSpan(0, 0, 4, 39, 0), "Miles Beyond", 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { -1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -1, 1, new TimeSpan(0, 0, 5, 43, 0), "Birds of Fire", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookletImages_ItemId",
                table: "BookletImages",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscArtists_DiscsId",
                table: "DiscArtists",
                column: "DiscsId");

            migrationBuilder.CreateIndex(
                name: "IX_Discs_ItemId",
                table: "Discs",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemArtists_ItemsId",
                table: "ItemArtists",
                column: "ItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_ArtistId",
                table: "Members",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtists_TracksId",
                table: "TrackArtists",
                column: "TracksId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_DiscId",
                table: "Tracks",
                column: "DiscId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookletImages");

            migrationBuilder.DropTable(
                name: "DiscArtists");

            migrationBuilder.DropTable(
                name: "ItemArtists");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "TrackArtists");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Discs");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
