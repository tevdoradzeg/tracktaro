namespace TrackTaro.Shared.Dtos;

public class ArtistDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public ICollection<MemberDto> Members { get; set; } = new List<MemberDto>();
}