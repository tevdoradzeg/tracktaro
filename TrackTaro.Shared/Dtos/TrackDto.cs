namespace TrackTaro.Shared.Dtos;

public class TrackDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int TrackNumber { get; set; }
}

public class CreateTrackDto
{
    public string Name { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int TrackNumber { get; set; }
}

public class UpdateTrackDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int TrackNumber { get; set; }
}