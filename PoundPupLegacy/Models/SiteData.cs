namespace PoundPupLegacy.Models;
public record SiteData
{
    public required Dictionary<int, UserWithDetails> Users { get; init; }
    public required Tenant Tenant { get; init; }
}
