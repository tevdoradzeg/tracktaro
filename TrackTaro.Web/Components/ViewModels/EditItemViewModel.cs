using TrackTaro.Web.Validation;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using TrackTaro.Shared;

namespace TrackTaro.Web.Components.ViewModels;

public class EditTrackViewModel
{
    public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public int TrackNumber { get; set; } = 1;
    [Required, TimeSpanFormat]
    public string Duration { get; set; } = "00:00:00";
}

public class EditDiscViewModel
{
    public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    public IBrowserFile? DiscImageFile { get; set; }
    [Required]
    public string ExistingDiscImagePath { get; set; } = string.Empty;
    [Required]
    public int Number { get; set; }
    [Required]
    public DiscType Type { get; set; }
    public List<EditTrackViewModel> Tracks { get; set; } = new();
}

public class EditItemViewModel
{
    public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [Required]
    [Range(1900, 2100)]
    public int Year { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public string Publisher { get; set; } = string.Empty;
    [Required]
    public string Label { get; set; } = string.Empty;
    [Required]
    public ItemType Type { get; set; }
    [Required]
    public List<EditDiscViewModel> Discs { get; set; } = new();
    public IBrowserFile? CoverImageFile { get; set; }
    public IBrowserFile? BackImageFile { get; set; }
    public List<IBrowserFile> BookletImageFiles { get; set; } = new();
    [Required]
    public string ExistingCoverImagePath { get; set; } = string.Empty;
    [Required]
    public string ExistingBackImagePath { get; set; } = string.Empty;
    [Required]
    public List<string> ExistingBookletImagePaths { get; set; } = new();
}