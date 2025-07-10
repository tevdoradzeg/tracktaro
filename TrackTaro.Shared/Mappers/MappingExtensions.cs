using TrackTaro.Shared;
using TrackTaro.Shared.Dtos;
using System.Linq;

namespace TrackTaro.Shared.Mappers;

public static class MappingExtensions
{
    public static TrackDto ToDto(this Track track)
    {
        return new TrackDto
        {
            Id = track.Id,
            Name = track.Name,
            TrackNumber = track.TrackNumber,
            Duration = track.Duration
        };
    }

    public static MemberDto ToDto(this Member member)
    {
        return new MemberDto
        {
            Id = member.Id,
            Name = member.Name
        };
    }

    public static ArtistDto ToDto(this Artist artist)
    {
        return new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            Country = artist.Country,
            Members = artist.Members.Select(m => m.ToDto()).ToList(),
            Items = artist.Items.Select(i => i.ToMinimalDto()).ToList(),
        };
    }

    public static ArtistShortDto ToShortDto(this Artist artist)
    {
        return new ArtistShortDto
        {
            Id = artist.Id,
            Name = artist.Name,
            Country = artist.Country,
            Members = artist.Members.Select(m => m.ToDto()).ToList()
        };
    }

    public static DiscDto ToDto(this Disc disc)
    {
        return new DiscDto
        {
            Id = disc.Id,
            DiscImagePath = disc.DiscImagePath,
            Number = disc.Number,
            Type = disc.Type,
            Artists = disc.Artists.Select(a => a.ToShortDto()).ToList(),
            Tracks = disc.Tracks.Select(t => t.ToDto()).ToList()
        };
    }

    public static BookletImageDto ToDto(this BookletImage bookletImage)
    {
        return new BookletImageDto
        {
            Id = bookletImage.Id,
            ImagePath = bookletImage.ImagePath
        };
    }

    public static ItemDto ToDto(this Item item)
    {
        return new ItemDto
        {
            Id = item.Id,
            Year = item.Year,
            Name = item.Name,
            Description = item.Description,
            Publisher = item.Publisher,
            Label = item.Label,
            Type = item.Type,
            CoverImagePath = item.CoverImagePath,
            BackImagePath = item.BackImagePath,
            CreatedAt = item.CreatedAt,
            Artists = item.Artists.Select(a => a.ToShortDto()).ToList(),
            Discs = item.Discs.Select(d => d.ToDto()).ToList(),
            BookletImages = item.BookletImages.Select(bI => bI.ToDto()).ToList()
        };
    }

    public static ItemShortDto ToShortDto(this Item item)
    {
        return new ItemShortDto
        {
            Id = item.Id,
            Year = item.Year,
            Name = item.Name,
            Description = item.Description,
            Publisher = item.Publisher,
            Label = item.Label,
            Type = item.Type,
            CoverImagePath = item.CoverImagePath,
            Artists = item.Artists.Select(a => a.ToShortDto()).ToList()
        };
    }
    
    public static ItemMinimalDto ToMinimalDto(this Item item)
    {
        return new ItemMinimalDto
        {
            Id = item.Id,
            Year = item.Year,
            Name = item.Name,
            Publisher = item.Publisher,
            Label = item.Label,
            Type = item.Type
        };
    }
}