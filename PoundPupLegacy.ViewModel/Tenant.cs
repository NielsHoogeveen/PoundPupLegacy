﻿namespace PoundPupLegacy.ViewModel;

public record Tenant
{
    public int Id { get; init; }

    public required string DomainName { get; init; }

    public required Dictionary<string, int> UrlToId { get; init; }
    public required Dictionary<int, string> IdToUrl { get; init; }
}
