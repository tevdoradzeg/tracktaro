using TrackTaro.Shared;

public class Track
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public int DiscNumber { get; set; } = 1;
    public int TrackNumber { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relations
    public int DiscId { get; set; }
    public virtual Disc Disc { get; set; } = null!;
    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
    public override string ToString() => $"{Name} ({Duration})";
}