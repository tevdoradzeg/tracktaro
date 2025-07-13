using System.ComponentModel.DataAnnotations;

namespace TrackTaro.Web.Validation;

public class TimeSpanFormatAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string durationString)
        {
            if (TimeSpan.TryParse(durationString, out _)) { return ValidationResult.Success; }
            return new ValidationResult("Invalid time format. Please use hh:mm:ss.", new[] { validationContext.MemberName! });
        }
        return new ValidationResult("Invalid input.", [validationContext.MemberName!]);
    }
}
