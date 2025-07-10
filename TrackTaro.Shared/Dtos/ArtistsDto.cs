namespace TrackTaro.Shared.Dtos;

public class ArtistDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public ICollection<ItemMinimalDto> Items { get; set; } = new List<ItemMinimalDto>();
    public ICollection<MemberDto> Members { get; set; } = new List<MemberDto>();
}

public class ArtistShortDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public ICollection<MemberDto> Members { get; set; } = new List<MemberDto>();
}

public class CreateArtistDto
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    // Since the dto would only contain member name use string instead
    public List<string> Members { get; set; } = new();
}