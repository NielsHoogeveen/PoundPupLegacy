﻿namespace PoundPupLegacy.Model;

public record SenateTerm: CongressionalTerm
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int? OwnerId { get; init; }
    public required int NodeTypeId { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }
    public required int? SenatorId { get; set; }
    public required int SubdivisionId { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }
    public required List<CongressionalTermPoliticalPartyAffiliation> PartyAffiliations { get; init; }
}
