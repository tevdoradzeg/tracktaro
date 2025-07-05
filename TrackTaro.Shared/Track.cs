using TrackTaro.Shared;

public class Track
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public int DiscNumber { get; set; } = 1;
    public int TrackNumber { get; set; } = 1;
    public Artist[] Artists { get; set; } = Array.Empty<Artist>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public override string ToString() => $"{Name} ({Duration})";
}