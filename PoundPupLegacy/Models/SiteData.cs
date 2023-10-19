namespace PoundPupLegacy.Models;
public record SiteData
{
    public required Dictionary<int, User> Users { get; init; }
    public required Tenant Tenant { get; init; }
}
