namespace PoundPupLegacy.Models;

public record Tenant
{
    public int Id { get; init; }

    public required string DomainName { get; init; }

    public required int CountryIdDefault { get; init; }

    public required string CountryNameDefault { get; init; }

    public required Dictionary<string, int> UrlToId { get; init; }
    public required Dictionary<int, string> IdToUrl { get; init; }
}
