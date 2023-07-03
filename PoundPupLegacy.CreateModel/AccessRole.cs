namespace PoundPupLegacy.CreateModel;

public sealed record AccessRole : UserRoleToCreate, PossiblyIdentifiable
{
    public required string Name { get; init; }

    public required int? UserGroupId { get; set; }

    public required Identification.Possible Identification { get; init; }

}
