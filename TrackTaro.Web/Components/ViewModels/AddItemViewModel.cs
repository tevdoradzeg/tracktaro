using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using TrackTaro.Shared;
using TrackTaro.Web.Validation;

namespace TrackTaro.Web.Components.ViewModels;

// Had to add GUIDS to these otherwise the model rerendering did not work correctly
public class AddTrackViewModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public int TrackNumber { get; set; } = 1;
    [Required]
    [TimeSpanFormat]
    public string Duration { get; set; } = "00:00:00";

}

public class AddDiscViewModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public int Number { get; set; } = 1;
    [Required]
    public DiscType Type { get; set; } = DiscType.CD;
    [Required]
    public IBrowserFile? DiscImageFile { get; set; }
    [Required]
    public List<AddTrackViewModel> Tracks { get; set; } = new();
}

public class AddItemViewModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    [Range(1900, 2100)]
    public int Year { get; set; } = DateTime.Now.Year;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public string Publisher { get; set; } = string.Empty;
    [Required]
    public string Label { get; set; } = string.Empty;
    [Required]
    public ItemType Type { get; set; } = ItemType.Album;
    [Required]
    public List<AddDiscViewModel> Discs { get; set; } = new();
    [Required]
    public IBrowserFile? CoverImageFile { get; set; }
    [Required]
    public IBrowserFile? BackImageFile { get; set; }
    [Required]
    public List<IBrowserFile> BookletImageFiles { get; set; } = new();
}