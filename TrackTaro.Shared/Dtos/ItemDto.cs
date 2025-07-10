namespace TrackTaro.Shared.Dtos;

public class ItemDto
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public ItemType Type { get; set; }
    public string CoverImagePath { get; set; } = string.Empty;
    public string BackImagePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ArtistDto> Artists { get; set; } = new List<ArtistDto>();
    public ICollection<DiscDto> Discs { get; set; } = new List<DiscDto>();
    public ICollection<BookletImageDto> BookletImages { get; set; } = new List<BookletImageDto>();
}

public class ItemShortDto
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public ItemType Type { get; set; }
    public string CoverImagePath { get; set; } = string.Empty;
    public ICollection<ArtistDto> Artists { get; set; } = new List<ArtistDto>();
}

public class BookletImageDto
{
    public int Id { get; set; }
    public string ImagePath { get; set; } = string.Empty;
}