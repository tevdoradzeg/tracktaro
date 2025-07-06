using Microsoft.EntityFrameworkCore;
using TrackTaro.Shared;

class MusicDbContext : DbContext
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
            .UsingEntity(j => j.ToTable("ItemArtists"));

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
    }
}