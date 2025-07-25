namespace TrackTaro.Shared.Dtos;

public class DiscDto
{
    public int Id { get; set; }
    public string DiscImagePath { get; set; } = string.Empty;
    public int Number { get; set; }
    public DiscType Type { get; set; }
    public ICollection<ArtistShortDto> Artists { get; set; } = new List<ArtistShortDto>();
    public ICollection<TrackDto> Tracks { get; set; } = new List<TrackDto>();
}

public class CreateDiscDto
{
    public string DiscImagePath { get; set; } = string.Empty;
    public int Number { get; set; }
    public DiscType Type { get; set; }
    public List<CreateTrackDto> Tracks { get; set; } = new();
}

public class UpdateDiscDto
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string DiscImagePath { get; set; } = string.Empty;
    public DiscType Type { get; set; }
    public List<UpdateTrackDto> Tracks { get; set; } = new();
}