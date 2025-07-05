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
    public Artist[] Artists { get; set; } = Array.Empty<Artist>();
    public Track[] Tracks { get; set; } = Array.Empty<Track>();
    public string DiscImagePath { get; set; } = string.Empty;
}

public class CDDisc : Disc
{
    override public DiscType Type => DiscType.CD;
}

public class VinylDisc : Disc
{
    override public DiscType Type => DiscType.Vinyl;
}