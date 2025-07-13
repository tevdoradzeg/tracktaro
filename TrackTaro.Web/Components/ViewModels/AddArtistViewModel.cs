using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using TrackTaro.Shared;
using TrackTaro.Web.Validation;

namespace TrackTaro.Web.Components.ViewModels;

public class AddArtistViewModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;

    [Required]
    public List<string> Members { get; set; } = new();
}