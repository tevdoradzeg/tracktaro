namespace TrackTaro.Shared.Dtos;

public class DiscDto
{
    public int Id { get; set; }
    public string DiscImagePath { get; set; } = string.Empty;
    public int Number { get; set; }
    public DiscType Type { get; set; }
    public ICollection<ArtistDto> Artists { get; set; } = new List<ArtistDto>();
    public ICollection<TrackDto> Tracks { get; set; } = new List<TrackDto>();
}

public class CreateDiscDto
{
    public string DiscImagePath { get; set; } = string.Empty;
    public int Number { get; set; }
    public DiscType Type { get; set; }
    public List<CreateTrackDto> Tracks { get; set; } = new();
}