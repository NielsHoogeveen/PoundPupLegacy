namespace PoundPupLegacy.CreateModel;

public sealed record ContentSharingGroup : Owner
{
    public required Identification.Possible IdentificationForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required AdministratorRole AdministratorRole { get; init; }

}
