using Microsoft.EntityFrameworkCore;
using TrackTaro.Shared;

class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options) {}

    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<Track> Tracks { get; set; } = null!;

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}