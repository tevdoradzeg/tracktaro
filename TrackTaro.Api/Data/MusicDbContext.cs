using Microsoft.EntityFrameworkCore;
using TrackTaro.Shared;

public class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options) {}

    // DbSets for the entities
    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<Track> Tracks { get; set; } = null!;
    public DbSet<Disc> Discs { get; set; } = null!;
    public DbSet<BookletImage> BookletImages { get; set; } = null!;
    public DbSet<Member> Members { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Explicit configuration for many-to-many relationships
        // As intructed in https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many

        // Item <-> Artist
        modelBuilder.Entity<Item>()
            .HasMany(item => item.Artists)
            .WithMany(artist => artist.Items)
            .UsingEntity<Dictionary<string, object>>(
                "ItemArtist",
                j => j.HasOne<Artist>().WithMany().HasForeignKey("ArtistsId"),
                j => j.HasOne<Item>().WithMany().HasForeignKey("ItemsId"),
                j =>
                {
                    j.ToTable("ItemArtists");

                    // Test data seeding here, see further below
                    j.HasData(new { ItemsId = -1, ArtistsId = -1 });
                }
            );

        // Track <-> Artist
        modelBuilder.Entity<Track>()
            .HasMany(t => t.Artists)
            .WithMany(a => a.Tracks)
            .UsingEntity(j => j.ToTable("TrackArtists"));

        // Disc <-> Artist
        modelBuilder.Entity<Disc>()
            .HasMany(d => d.Artists)
            .WithMany(a => a.Discs)
            .UsingEntity(j => j.ToTable("DiscArtists"));

        // Workaround to make EF see Disc children, otherwise I get a build error
        modelBuilder.Entity<Disc>()
            .HasDiscriminator(d => d.Type)
            .HasValue<CDDisc>(DiscType.CD)
            .HasValue<VinylDisc>(DiscType.Vinyl);


        // Seeding with test data (with one of the best prog jazz albums oat)

        // otw EF throws an error when updating
        var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Artist>().HasData(
            new Artist { Id = -1, Name = "Mahavishnu Orchestra", Country = "United States", CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        modelBuilder.Entity<Item>().HasData(
            new Item { Id = -1, Name = "Birds of Fire", Year = 1973, Type = ItemType.Album, Description = "As with the group's previous album, The Inner Mounting Flame, Birds of Fire consists solely of compositions by John McLaughlin. These include the track \"Miles Beyond (Miles Davis)\", which McLaughlin dedicated to his friend and former bandleader.", CreatedAt = seedDate, UpdatedAt = seedDate } // Descritpion sourced from Wikipedia
        );

        modelBuilder.Entity<CDDisc>().HasData(
            new CDDisc { Id = -1, ItemId = -1, Number = 1, DiscImagePath = "/fakepath.png", CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        modelBuilder.Entity<Track>().HasData(
            new Track { Id = -1, DiscId = -1, TrackNumber = 1, Name = "Birds of Fire", Duration = new TimeSpan(0, 5, 43), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Track { Id = -2, DiscId = -1, TrackNumber = 2, Name = "Miles Beyond", Duration = new TimeSpan(0, 4, 39), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Track { Id = -3, DiscId = -1, TrackNumber = 3, Name = "Celestial Terrestrial Commuters", Duration = new TimeSpan(0, 2, 54), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Track { Id = -4, DiscId = -1, TrackNumber = 4, Name = "Sapphire Bullets of Pure Love", Duration = new TimeSpan(0, 0, 22), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Track { Id = -5, DiscId = -1, TrackNumber = 5, Name = "Thousand Islad Park", Duration = new TimeSpan(0, 3, 20), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Track { Id = -6, DiscId = -1, TrackNumber = 6, Name = "Hope", Duration = new TimeSpan(0, 1, 57), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Track { Id = -7, DiscId = -1, TrackNumber = 7, Name = "One Word", Duration = new TimeSpan(0, 9, 55), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Track { Id = -8, DiscId = -1, TrackNumber = 8, Name = "Sanctuary", Duration = new TimeSpan(0, 5, 2), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Track { Id = -9, DiscId = -1, TrackNumber = 9, Name = "Open Country Joy", Duration = new TimeSpan(0, 3, 54), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Track { Id = -10, DiscId = -1, TrackNumber = 10, Name = "Resolution", Duration = new TimeSpan(0, 2, 10), CreatedAt = seedDate, UpdatedAt = seedDate }
        );
    }
}