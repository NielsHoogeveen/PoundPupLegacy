namespace PoundPupLegacy.Model;

public record UserRole: AccessRole
{
    public required int? Id { get; set; }

    public required string Name { get; init; }

}
