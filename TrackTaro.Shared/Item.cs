namespace TrackTaro.Shared;

public enum ItemType
{
    Album,
    Single,
    EP,
    Compilation,
    BoxSet,
    Other
}

public class Item
{
    public int Id { get; set; }
    public int Year { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Disc[] Discs { get; set; } = Array.Empty<Disc>();
    public Artist[] Artists { get; set; } = Array.Empty<Artist>();
    public ItemImages Images { get; set; } = new ItemImages();
    public ItemType Type { get; set; } = ItemType.Album;
    public override string ToString() => $"{Name} - {Description}";
}

public class ItemImages
{
    public string CoverImagePath { get; set; } = string.Empty;
    public string BackImagePath { get; set; } = string.Empty;
    public string[] BookletImagePaths { get; set; } = Array.Empty<string>();
}