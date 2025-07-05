using TrackTaro.Shared;

public class Member
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ArtistId { get; set; }
    public virtual Artist Artist { get; set; } = null!;

    public override string ToString() => Name;
}