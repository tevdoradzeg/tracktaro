namespace TrackTaro.Shared;

public enum DiscType
{
    CD,
    Vinyl
}

public abstract class Disc
{
    public int Id { get; set; }
    public abstract DiscType Type { get; }
    public int Number { get; set; } = 1;
    public string DiscImagePath { get; set; } = string.Empty;
    public int ItemId { get; set; }
    public virtual Item Item { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relations
    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}

public class CDDisc : Disc
{
    override public DiscType Type => DiscType.CD;
}

public class VinylDisc : Disc
{
    override public DiscType Type => DiscType.Vinyl;
}