﻿using PoundPupLegacy.Common;
using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

[JsonSerializable(typeof(Tenant))]
internal partial class TenantJsonContext : JsonSerializerContext { }

public sealed record Tenant: ITenant
{
    public int Id { get; init; }
    public required string Name { get; init; }

    public required string Title { get; init; }

    public required string Description { get; init; }

    public required string DomainName { get; init; }

    public required int CountryIdDefault { get; init; }

    public required string CountryNameDefault { get; init; }

    public required string? FrontPageText { get; init; }

    public required string? Logo { get; init; }

    public required string? Subtitle { get; init; }

    public required string? FooterText { get; init; }

    public required string? CssFile { get; init; }

    public required int FrontPageId{ get; init; }

    public required string? GoogleAnalyticsMeasurementId { get; init; }
    public required string? RegistrationText { get; init; }
    public required bool TrackActiveUsers { get; init; }
    public required SmtpConnection SmtpConnection { get; init; }

    private List<TenantNode> _tenantNodes = new();
    public required List<TenantNode> TenantNodes {
        get => _tenantNodes;
        init { 
            if(value is not null) {
                _tenantNodes = value;
            }
        }
    }

    public Dictionary<string, int> UrlToId { get; set; } = new Dictionary<string, int>();

    public Dictionary<int, string> IdToUrl { get; set; } = new Dictionary<int, string>();
}
