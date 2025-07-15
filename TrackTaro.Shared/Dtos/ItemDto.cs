using System.ComponentModel.DataAnnotations;

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
    public ICollection<ArtistShortDto> Artists { get; set; } = new List<ArtistShortDto>();
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
    public ICollection<ArtistShortDto> Artists { get; set; } = new List<ArtistShortDto>();
}

public class ItemMinimalDto
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public ItemType Type { get; set; }
}

public class CreateItemDto
{
    public int Year { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public ItemType Type { get; set; }
    public string CoverImagePath { get; set; } = string.Empty;
    public string BackImagePath { get; set; } = string.Empty;
    public List<CreateDiscDto> Discs { get; set; } = new();
    // since dto would only contain path use string instead
    public List<string> BookletImagePaths { get; set; } = new();
}

public class UpdateItemDto
{
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public ItemType Type { get; set; }
    public string CoverImagePath { get; set; } = string.Empty;
    public string BackImagePath { get; set; } = string.Empty;
    public List<string> BookletImagePaths { get; set; } = new();
    public List<UpdateDiscDto> Discs { get; set; } = new();
    public List<int> ArtistIds { get; set; } = new();
}

public class BookletImageDto
{
    public int Id { get; set; }
    public string ImagePath { get; set; } = string.Empty;
}