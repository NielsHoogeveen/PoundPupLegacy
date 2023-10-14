namespace PoundPupLegacy.Models;

public record SiteMapElement
{
    public required string Path { get; init; } 

    public DateTime? LastChanged { get; init; }

    public string? ChangeFrequency { get; init; }
}
