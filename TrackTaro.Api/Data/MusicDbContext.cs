using Microsoft.EntityFrameworkCore;

class MusicDbContext : DbContext
{
    // Temp placeholder class before actual implementation
    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}