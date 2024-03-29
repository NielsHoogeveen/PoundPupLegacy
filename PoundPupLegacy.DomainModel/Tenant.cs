﻿namespace PoundPupLegacy.DomainModel;

public sealed record Tenant : Owner, PublishingUserGroup
{
    public required Identification.Possible Identification { get; init; }
    public required int PublicationStatusIdDefault { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string? FrontPageText { get; init; }
    public required string? SubTitle { get; init; }
    public required string? Logo { get; init; }
    public required string? FooterText { get; init; }
    public required string? CssFile { get; init; }
    public required string DomainName { get; init; }
    public required int? CountryIdDefault { get; init; }
    public required AccessRole AccessRoleNotLoggedIn { get; init; }
    public required AdministratorRole AdministratorRole { get; init; }
}
