namespace PoundPupLegacy.DomainModel;

public sealed record ContentSharingGroup : Owner
{
    public required Identification.Possible Identification { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required AdministratorRole AdministratorRole { get; init; }

}
