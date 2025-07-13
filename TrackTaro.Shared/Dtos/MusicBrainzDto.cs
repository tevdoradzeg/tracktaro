using System.Text.Json.Serialization;

namespace TrackTaro.Shared.Dtos;

public class MusicBrainzSearchResult
{
    [JsonPropertyName("artists")]
    public List<MusicBrainzArtist> Artists { get; set; } = new();
}

public class MusicBrainzArtist
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string? CountryCode { get; set; }


    [JsonPropertyName("area")]
    public MusicBrainzArea? Area { get; set; }

    [JsonPropertyName("disambiguation")]
    public string? Disambiguation { get; set; } // e.g., "author" or "the band"
}

public class MusicBrainzArea
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
}

public class ArtistSuggestionDto
{
    public string Name { get; set; } = "";
    public string Country { get; set; } = "";
    public string Disambiguation { get; set; } = "";
}