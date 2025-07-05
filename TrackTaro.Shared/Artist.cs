namespace TrackTaro.Shared;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string[] Members { get; set; } = Array.Empty<string>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relations
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    public virtual ICollection<Disc> Discs { get; set; } = new List<Disc>();
    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

    public override string ToString() => Name;
}