using System.Text.Json.Serialization;

namespace MyCustomUmbracoProject.Models;

public class DateOnlyValue
{
    [JsonPropertyName("date")]
    public DateTimeOffset Date { get; init; }
}
