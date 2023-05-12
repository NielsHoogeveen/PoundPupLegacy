﻿namespace PoundPupLegacy.CreateModel;

public sealed record OrganizationType : Nameable
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int OwnerId { get; init; }
    public required int AuthoringStatusId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Description { get; init; }
    public required int? FileIdTileImage { get; init; }
    public required List<VocabularyName> VocabularyNames { get; init; }
    public required bool HasConcreteSubtype { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }

}
