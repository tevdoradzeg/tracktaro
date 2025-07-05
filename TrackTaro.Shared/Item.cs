namespace TrackTaro.Shared;
using System.ComponentModel.DataAnnotations;

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
    public ItemType Type { get; set; } = ItemType.Album;

    // Image management for the item
    public string CoverImagePath { get; set; } = string.Empty;
    public string BackImagePath { get; set; } = string.Empty;

    // Relations
    public virtual ICollection<Disc> Discs { get; set; } = new List<Disc>();
    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
    public virtual ICollection<BookletImage> BookletImages { get; set; } = new List<BookletImage>();

    public override string ToString() => $"{Name} - {Description}";
}

public class BookletImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
    }