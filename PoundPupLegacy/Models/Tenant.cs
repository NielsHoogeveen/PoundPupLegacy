using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

[JsonSerializable(typeof(Tenant))]
internal partial class TenantJsonContext : JsonSerializerContext { }

public sealed record Tenant
{
    public int Id { get; init; }

    public required string DomainName { get; init; }

    public required int CountryIdDefault { get; init; }

    public required string CountryNameDefault { get; init; }

    public required string? FrontPageText { get; init; }

    public required string? Logo { get; init; }

    public required string? SubTitle { get; init; }

    public required string? FooterText { get; init; }

    public required string? CssFile { get; init; }

    public required Dictionary<string, int> UrlToId { get; init; }

    public required Dictionary<int, string> IdToUrl { get; init; }
}
